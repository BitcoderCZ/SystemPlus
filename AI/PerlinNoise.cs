using System;
using System.Drawing;
using SystemPlus.Vectors;

namespace SystemPlus.AI
{
    public static class PerlinNoise
    {
        public static float[][] GenerateWhiteNoise(int width, int height)
        {
            System.Random random = new System.Random((int)((float)DateTime.Now.Millisecond *
                ((float)DateTime.Now.Millisecond / 0.2f)));
            float[][] noise = new float[width][];
            for (int i = 0; i < width; i++)
                noise[i] = new float[height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    noise[x][y] = (float)random.NextDouble() % 1;

            return noise;
        }

        public static float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][] smoothNoise = new float[width][];
            for (int i = 0; i < width; i++)
                smoothNoise[i] = new float[height];

            int samplePeriod = 2 ^ octave; // calculates 2 ^ k
            float sampleFrequency = 1.0f / 6;

            for (int i = 0; i < width; i++)
            {
                //calculate the horizontal sampling indices
                int sample_i0 = (i / 6) * 6;
                int sample_i1 = (sample_i0 + 6) % width; //wrap around
                float horizontal_blend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    //calculate the vertical sampling indices
                    int sample_j0 = (j / 6) * 6;
                    int sample_j1 = (sample_j0 + 6) % height; //wrap around
                    float vertical_blend = (j - sample_j0) * sampleFrequency;

                    //blend the top two corners
                    float top = Interpolate(baseNoise[sample_i0][sample_j0],
                       baseNoise[sample_i1][sample_j0], horizontal_blend);

                    //blend the bottom two corners
                    float bottom = Interpolate(baseNoise[sample_i0][sample_j1],
                       baseNoise[sample_i1][sample_j1], horizontal_blend);

                    //final blend
                    smoothNoise[i][j] = Interpolate(top, bottom, vertical_blend);
                }
            }

            return smoothNoise;
        }

        static float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][][] smoothNoise = new float[octaveCount][][]; //an array of 2D arrays containing

            float persistance = 0.5f;

            //generate smooth noise
            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            float[][] perlinNoise = new float[width][];
            for (int i = 0; i < width; i++)
                perlinNoise[i] = new float[height];
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            //blend noise together
            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                    }
                }
            }

            //normalisation
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }

        public static Color[][] BlendImages(Color[][] image1, Color[][] image2, float[][] perlinNoise)
        {
            int width = image1.Length;
            int height = image1[0].Length;

            Color[][] image = new Color[width][];
            for (int i = 0; i < width; i++)
                image[i] = new Color[height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    image[i][j] = Color.FromArgb((byte)(int)(Interpolate(image1[i][j].R, image2[i][j].R, perlinNoise[i][j]) * 255),
                        (byte)(int)(Interpolate(image1[i][j].G, image2[i][j].G, perlinNoise[i][j]) * 255),
                        (byte)(int)(Interpolate(image1[i][j].B, image2[i][j].B, perlinNoise[i][j]) * 255));

                }
            }

            return image;
        }

        static Color GetColor(Color gradientStart, Color gradientEnd, float t)
        {
            float u = 1 - t;

            Color color = Color.FromArgb(
               255,
               (byte)(int)(gradientStart.R * u + gradientEnd.R * t),
               (byte)(int)(gradientStart.G * u + gradientEnd.G * t),
               (byte)(int)(gradientStart.B * u + gradientEnd.B * t));

            return color;
        }

        static Color[][] MapGradient(Color gradientStart, Color gradientEnd, float[][] perlinNoise)
        {
            int width = perlinNoise.Length;
            int height = perlinNoise[0].Length;

            Color[][] image = new Color[width][];
            for (int i = 0; i < width; i++)
                image[i] = new Color[height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    image[i][j] = GetColor(gradientStart, gradientEnd, perlinNoise[i][j]);
                }
            }

            return image;
        }

        static float[][] AdjustLevels(float[][] image, float low, float high)
        {
            int width = image.Length;
            int height = image[0].Length;

            float[][] newImage = new float[width][];
            for (int i = 0; i < width; i++)
                image[i] = new float[height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float col = image[i][j];

                    if (col <= low)
                    {
                        newImage[i][j] = 0;
                    }
                    else if (col >= high)
                    {
                        newImage[i][j] = 1;
                    }
                    else
                    {
                        newImage[i][j] = (col - low) / (high - low);
                    }
                }
            }

            return newImage;
        }

        public static float ValueAt(int x, int y, int octave = 1, int octaveCount = 20)
        {
            return GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, y + 5), 1), 20)[x][y];
        }

        public static float ValueAtPoint(int x, int y, int z, int octave = 1, int octaveCount = 20)
        {
            float XY = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, y + 5), 1), 20)[x][y];
            float XZ = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, y + 5), 1), 20)[x][z];
            float YZ = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, y + 5), 1), 20)[y][z];

            return (XY + XZ + YZ) / 3f;
        }

        public static float ValueAtPoint(Vector3Int pos, int octave = 1, int octaveCount = 20)
        {
            int x = pos.x;
            int y = pos.y;
            int z = pos.z;
            float XY = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, y + 5), 1), 20)[x][y];
            float XZ = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(x + 5, z + 5), 1), 20)[x][z];
            float YZ = GeneratePerlinNoise(GenerateSmoothNoise(GenerateWhiteNoise(y + 5, z + 5), 1), 20)[y][z];

            return (XY + XZ + YZ) / 3f;
        }

        public static void Save(float[][] pixels, string Location)
        {
            Bitmap bm = new Bitmap(pixels.Length, pixels[0].Length);
            for (int x = 0; x < pixels.Length; x++)
            {
                for (int y = 0; y < pixels[0].Length; y++)
                {
                    int pixelColor = int.Parse(Math.Round(pixels[x][y] * 255f).ToString());
                    bm.SetPixel(x, y, Color.FromArgb(0, (byte)pixelColor, (byte)pixelColor));
                }
            }
            bm.Save(Location);
        }
    }
}
