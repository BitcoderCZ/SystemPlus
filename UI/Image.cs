using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SystemPlus.ClassSupport;
using SystemPlus.Extensions;
using SystemPlus.Utils;
using SystemPlus.Vectors;

namespace SystemPlus.UI
{
    public sealed class Image : SaveSupport, IDisposable
    {
        public Int32[] pixels { get; private set; }

        public Int32 Width { get; private set; }
        public Int32 Height { get; private set; }

        #region ctors
        public Image() { }
        private Image(Int32 _width, Int32[] _pixels)
        {
            Width = _width;
            pixels = _pixels;
            Height = _pixels.GetHeight(Width);
            disposed = false;
        }

        /// <summary>
        ///  Suported formats: .png, .jpg, .jpeg, .catmg32, .catmg8
        /// </summary>
        public static Image Create(string location)
        {
            if (!File.Exists(location))
                return null;

            switch (System.IO.Path.GetExtension(location).ToLower())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    return FromImage(location);
                case ".catmg32":
                    return FromCATMG32(location);
                case ".catmg8":
                    return FromCATMG8(location);
                default:
                    return null;
            }
        }

        /// <summary>
        ///  Suported formats: .png, .jpg, .jpeg, .catmg32, .catmg8
        /// </summary>
        public static Image Create(FileStream stream)
        {
            if (stream == null)
                return null;

            switch (System.IO.Path.GetExtension(stream.Name).ToLower())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    return FromImage(stream);
                case ".catmg32":
                    return FromCATMG32(stream);
                case ".catmg8":
                    return FromCATMG8(stream);
                default:
                    return null;
            }
        }

        public static Image Create(Int32 width, Int32[] pixels)
        {
            if (pixels == null)
                return null;

            return new Image(width, pixels);
        }

        public static Image Create(Vector2Int size)
        {
            Int32[] pixels = new Int32[size.x * size.y];
            return new Image(size.x, pixels);
        }

        public static Image Create(int x, int y) => Create(new Vector2Int(x, y));

        private static unsafe int[] BmpToBytes_Unsafe(Bitmap bmp)
        {
            BitmapData bData = bmp.LockBits(new Rectangle(new System.Drawing.Point(), bmp.Size),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppRgb);

            // number of bytes in the bitmap
            int byteCount = bData.Stride * (bmp.Height);

            int[] bytes = new int[byteCount / 4];

            Marshal.Copy(bData.Scan0, bytes, 0, byteCount / 4);

            // don't forget to unlock the bitmap!!
            bmp.UnlockBits(bData);

            return bytes;
        }

        private static Image FromImage(string location)
        {
            Bitmap bm = new Bitmap(location);
            Int32[] pixels = BmpToBytes_Unsafe(bm);

            return new Image(bm.Width, pixels);
        }

        private static Image FromImage(FileStream stream)
        {
            Bitmap bm = new Bitmap(stream);
            Int32[] pixels = BmpToBytes_Unsafe(bm);

            return new Image(bm.Width, pixels);
            /*Bitmap bm = new Bitmap(stream);
            Int32[] pixels = new Int32[bm.Width * bm.Height];
            for (int x = 0; x < bm.Width; x++)
                for (int y = 0; y < bm.Width; y++)
                    pixels[y * bm.Width + x] = ((Color)bm.GetPixel(x, y)).ToInt32();

            return new Image(bm.Width, pixels);*/
        }

        private static Image FromCATMG32(FileStream stream)
        {
            try
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                if (bytes.Length % 4 != 0)
                    return null;
                int index = 0;
                Int32 width = bytes.ReadInt(ref index);
                Int32[] pixels = new Int32[(bytes.Length - 4) / 4];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = bytes.ReadInt(ref index);
                return new Image(width, pixels);
            }
            catch { return null; }
        }

        private static Image FromCATMG32(string location)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(location);
                if (bytes.Length % 4 != 0)
                    return null;
                int index = 0;
                Int32 width = bytes.ReadInt(ref index);
                Int32[] pixels = new Int32[(bytes.Length - 4) / 4];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = bytes.ReadInt(ref index);
                return new Image(width, pixels);
            }
            catch { return null; }
        }

        private static Image FromCATMG8(FileStream stream)
        {
            try
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                int index = 0;
                Int32 width = bytes.ReadInt(ref index);
                Int32[] pixels = new Int32[bytes.Length - 4];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = Color.From8(bytes[i]).To32();
                return new Image(width, pixels);
            }
            catch { return null; }
        }

        private static Image FromCATMG8(string location)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(location);
                int index = 0;
                Int32 width = bytes.ReadInt(ref index);
                Int32[] pixels = new Int32[bytes.Length - 4];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = Color.From8(bytes[i]).To32();
                return new Image(width, pixels);
            }
            catch { return null; }
        }
        #endregion

        [Obsolete("Just dont use this, to slow...")]
        public Image Resize(int newWidth, int newHeight)
        {
            if (newWidth < 0 || newHeight < 0)
                throw new Exception("Size can't be nagative!");

            float wSizer = (float)newWidth / (float)Width;
            float hSizer = (float)newHeight / (float)Height;

            #region calculate width

            Vector4[] valuesW = new Vector4[Width * Height];

            Int32[] newPixelsW = new Int32[newWidth * Height];
            int y = 0;
            int x = -1;
            for (int i = 0; i < valuesW.Length; i++)
            {
                x++;
                if (x == Width)
                {
                    x = 0;//int x = ArrayExtensions.GetX(i, Width);
                          //if (x == 0)
                    y++;
                }

                Vector4 v = new Vector4(x, (x + 1) * wSizer,
                    y/*ArrayExtensions.GetY(i, Width, Height)*/, pixels[i]);
                valuesW[i] = v;
                //}
            }

            y = 0;
            x = -1;
            for (int i = 0; i < newPixelsW.Length; i++)
            {
                x++;
                if (x == newWidth)
                {
                    x = 0;//int x = ArrayExtensions.GetX(i, Width);
                          //if (x == 0)
                    y++;
                }
                Int32[] colors = valuesW.FindAll((Vector4 v) =>
                {
                    if (v.z == y && x >= v.x && x <= v.y)
                        return true;
                    else
                        return false;
                }).ToNewType((Vector4 v4) => { return (int)v4.w; });
                if (colors == null)
                {
                    colors = new Int32[1];
                    colors[0] = Color.FromARGB(255, 0, 0, 0).To32();
                }
                else if (colors.Length == 0)
                {
                    colors = new Int32[1];
                    colors[0] = Color.FromARGB(255, 0, 0, 0).To32();
                }

                int a = 0;
                int r = 0;
                int g = 0;
                int b = 0;

                for (int j = 0; j < colors.Length; j++)
                {
                    Color clr = Color.FromARGB(colors[j]);
                    a += clr.A;
                    r += clr.R;
                    g += clr.G;
                    b += clr.B;
                }

                a /= colors.Length;
                r /= colors.Length;
                g /= colors.Length;
                b /= colors.Length;

                newPixelsW[i] = Color.To32((byte)a, (byte)r, (byte)g, (byte)b);
            }
            #endregion
            Vector4[] values = new Vector4[newWidth * Height];

            Int32[] newPixels = new Int32[newWidth * newHeight];
            y = 0;
            x = -1;
            for (int i = 0; i < values.Length; i++)
            {
                x++;
                if (x == newWidth)
                {
                    x = 0;//int x = ArrayExtensions.GetX(i, Width);
                          //if (x == 0)
                    y++;
                }

                Vector4 v = new Vector4(y, (y + 1) * hSizer,
                    x/*ArrayExtensions.GetY(i, Width, Height)*/, newPixelsW[i]);
                values[i] = v;
                //}
            }

            y = 0;
            x = -1;
            for (int i = 0; i < newPixels.Length; i++)
            {
                x++;
                if (x == newWidth)
                {
                    x = 0;//int x = ArrayExtensions.GetX(i, Width);
                          //if (x == 0)
                    y++;
                }
                Int32[] colors = values.FindAll((Vector4 v) =>
                {
                    if (v.z == x && y >= v.x && y <= v.y)
                        return true;
                    else
                        return false;
                }).ToNewType((Vector4 v4) => { return (int)v4.w; });
                if (colors == null)
                {
                    colors = new Int32[1];
                    colors[0] = Color.FromARGB(255, 255, 255, 255).To32();
                }
                else if (colors.Length == 0)
                {
                    Console.WriteLine(0);
                    colors = new Int32[1];
                    colors[0] = Color.FromARGB(255, 255, 0, 0).To32();
                }

                Console.WriteLine(colors.Length);
                /*int a = 0;
                int r = 0;
                int g = 0;
                int b = 0;

                for (int j = 0; j < colors.Length; j++)
                {
                    Color clr = Color.FromARGB(colors[j]);
                    a += clr.A;
                    r += clr.R;
                    g += clr.G;
                    b += clr.B;
                }

                a /= colors.Length;
                r /= colors.Length;
                g /= colors.Length;
                b /= colors.Length;*/

                newPixels[i] = colors[0];//Color.To32((byte)a, (byte)r, (byte)g, (byte)b);
            }

            return new Image(newWidth, newPixels);
        }

        #region get_set
        public Color GetPixel(Vector2Int pos)
        {
            try
            {
                if (!pixels.InBounds(pos, Width))
                    throw new IndexOutOfRangeException($"Pos({pos}) not in bounds!");
                else
                    return Color.FromARGB(pixels[pos.y * Width + pos.x]);
            }
            catch
            {
                return Color.FromARGB(255, 0, 0, 0);
            }
        }

        public Int32 GetPixel32(Vector2Int pos)
        {
            try
            {
                if (!pixels.InBounds(pos, Width))
                    throw new IndexOutOfRangeException($"Pos({pos}) not in bounds!");
                else
                    return pixels[pos.y * Width + pos.x];
            }
            catch
            {
                return Color.FromARGB(255, 0, 0, 0).To32();
            }
        }

        public Color GetPixel(int x, int y)
        {
            try
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    throw new IndexOutOfRangeException($"Pos({x}:{y}) not in bounds!");
                else
                    return Color.FromARGB(pixels[y * Width + x]);
            }
            catch
            {
                return Color.FromARGB(255, 0, 0, 0);
            }
        }

        public Int32 GetPixel32(int x, int y)
        {
            try
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    throw new IndexOutOfRangeException($"Pos({x}:{y}) not in bounds!");
                else
                    return pixels[y * Width + x];
            }
            catch
            {
                return Color.FromARGB(255, 0, 0, 0).To32();
            }
        }

        public bool SetPixel(Vector2Int pos, Color clr)
        {
            if (!pixels.InBounds(pos, Width))
                return false;
            else
            {
                pixels[pos.y * Width + pos.x] = clr.To32();
                return true;
            }
        }

        public bool SetPixel(int x, int y, Color clr)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
            else
            {
                pixels[y * Width + x] = clr.To32();
                return true;
            }
        }

        public bool SetPixel(Vector2Int pos, Int32 clr)
        {
            if (!pixels.InBounds(pos, Width))
                return false;
            else
            {
                pixels[pos.y * Width + pos.x] = clr;
                return true;
            }
        }

        public bool SetPixel(int x, int y, Int32 clr)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
            else
            {
                pixels[y * Width + x] = clr;
                return true;
            }
        }
        #endregion

        public static Int32[] GetPixels(Image image) => (Int32[])image.pixels.Clone();

        public void Print()
        {
            ConsoleColor[] consolePixels = new ConsoleColor[pixels.Length];
            Parallel.For(0, pixels.Length, (int i) =>
            {
                consolePixels[i] = ColorExtensions.ClosestConsoleColor(Color.FromARGB(pixels[i]));
            });

            int XStart = Console.CursorLeft;

            ConsoleColor lastColor = consolePixels[0];
            Console.BackgroundColor = consolePixels[0];
            string toPrint = "";

            for (int i = 0; i < pixels.Length; i++)
            {
                if (i % Width == 0 && i != 0 && toPrint != "")
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = consolePixels[i];
                    Console.SetCursorPosition(XStart, Console.CursorTop + 1);
                    continue;
                }

                if (consolePixels[i] == lastColor)
                {
                    toPrint += "  ";
                }
                else
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = consolePixels[i];
                }
            }

            if (toPrint != "")
            {
                Console.BackgroundColor = lastColor;
                Console.Write(toPrint);
            }
            Console.Write("\n");
        }

        protected override void save(string path)
        {
            switch (System.IO.Path.GetExtension(path).ToLower())
            {
                case ".catmg32":
                    FileSave save32 = new FileSave();
                    save32.WriteInt(Width);
                    for (int i = 0; i < pixels.Length; i++)
                        save32.WriteInt(pixels[i]);
                    File.WriteAllBytes(path, save32.Bytes);
                    break;
                case ".catmg8":
                    FileSave save8 = new FileSave();
                    save8.WriteInt(Width);
                    for (int i = 0; i < pixels.Length; i++)
                        save8.WriteByte(Color.FromARGB(pixels[i]).To8());
                    File.WriteAllBytes(path, save8.Bytes);
                    break;
                case ".png":
                    DirectBitmap db = new DirectBitmap(Width, Height);
                    for (int x = 0; x < Width; x++)
                        for (int y = 0; y < Height; y++)
                            db.Write(x, y, GetPixel32(x, y));
                    db.Bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    db.Dispose();
                    break;
                case ".jpg":
                case ".jpeg":
                    DirectBitmap dbJ = new DirectBitmap(Width, Height);
                    for (int x = 0; x < Width; x++)
                        for (int y = 0; y < Height; y++)
                            dbJ.Write(x, y, GetPixel32(x, y));
                    dbJ.Bitmap.Save(path, ImageFormat.Jpeg);
                    dbJ.Dispose();
                    break;
                default:
                    break;
            }
        }

        protected override void load(ref object loadedObject, string location)
        {
            loadedObject = Create(location);
        }

        protected override string getSuportedExtensions() => ".png, .jpg, .jpeg, .catmg32, .catmg8; (when loading, extensions converted ToLower)";


        public bool disposed { get; private set; }

        public void Dispose()
        {
            if (disposed == true)
                return;

            disposed = true;
            pixels = default;
            /*GC.SuppressFinalize(pixels);
            GC.SuppressFinalize(width);
            GC.SuppressFinalize(height);*/
        }

        ~Image()
        {
            Dispose();
        }
    }
}
