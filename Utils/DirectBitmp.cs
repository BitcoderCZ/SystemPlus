﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace SystemPlus.Utils
{
    public class DirectBitmap :
        Buffer2D<int>
    {
        #region storage

        public Bitmap Bitmap { get; private set; }

        public Graphics Graphics { get; private set; }

        #endregion

        #region ctor

        public DirectBitmap(Size size, int[] data) :
            base(size, data)
        {
            Bitmap = new Bitmap(Width, Height, Width * sizeof(int), PixelFormat.Format32bppPArgb, Address);
            Graphics = Graphics.FromImage(Bitmap);
        }

        public DirectBitmap(Size size) :
            this(size, new int[size.Width * size.Height])
        {
        }

        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        public override void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            base.Dispose();
        }

        #endregion

        #region routines

        public static unsafe DirectBitmap Load(string path, bool log = true)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File doesnt exists", path);

            DateTime start = DateTime.Now;
            if (log)
                Console.WriteLine("Loading " + path);

            Bitmap srcBitmap = new Bitmap(path);
            DirectBitmap db = new DirectBitmap(srcBitmap.Size);

            Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
            BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);//srcBitmap.PixelFormat);
            BitmapData resData = db.Bitmap.LockBits(bmpBounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);//db.Bitmap.PixelFormat);

            int* srcScan0 = (int*)srcData.Scan0;
            int* resScan0 = (int*)resData.Scan0;
            int numPixels = srcData.Stride / 4 * srcData.Height;
            try
            {
                for (int p = 0; p < numPixels; p++)
                {
                    resScan0[p] = srcScan0[p];
                }
            }
            finally
            {
                srcBitmap.UnlockBits(srcData);
                db.Bitmap.UnlockBits(resData);
            }

            if (log)
                Console.WriteLine("Loaded " + path + " in " + (DateTime.Now - start).TotalSeconds + " seconds");

            srcBitmap.Dispose();

            return db;
        }

        public static DirectBitmap LoadSafe(string path, bool log = true)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File doesnt exists", path);

            DateTime start = DateTime.Now;
            if (log)
                Console.WriteLine("Loading " + path);

            Bitmap b = new Bitmap(path);

            DirectBitmap db = new DirectBitmap(b.Size);

            for (int i = 0; i < db.Width * db.Height; i++)
                db.Write(i, b.GetPixel(i % db.Width, i / db.Width).ToArgb());

            if (log)
                Console.WriteLine("Loaded " + path + " in " + (DateTime.Now - start).TotalSeconds + " seconds");

            return db;
        }

        public static unsafe DirectBitmap LoadFromBm(Bitmap srcBitmap, bool log = true)
        {
            DateTime start = DateTime.Now;
            if (log)
                Console.WriteLine("Loading DB");

            DirectBitmap db = new DirectBitmap(srcBitmap.Size);

            Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
            BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resData = db.Bitmap.LockBits(bmpBounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int* srcScan0 = (int*)srcData.Scan0;
            int* resScan0 = (int*)resData.Scan0;
            int numPixels = srcData.Stride / 4 * srcData.Height;
            try {
                for (int p = 0; p < numPixels; p++) {
                    resScan0[p] = srcScan0[p];
                }
            }
            finally {
                srcBitmap.UnlockBits(srcData);
                db.Bitmap.UnlockBits(resData);
            }

            if (log)
                Console.WriteLine("Loaded DB in " + (DateTime.Now - start).TotalSeconds + " seconds");

            return db;
        }

        public static DirectBitmap LoadSafeFromBm(Bitmap srcBitmap, bool log = true)
        {
            DateTime start = DateTime.Now;
            if (log)
                Console.WriteLine("Loading DB");

            DirectBitmap db = new DirectBitmap(srcBitmap.Size);

            for (int x = 0; x < srcBitmap.Width; x++)
                for (int y = 0; y < srcBitmap.Height; y++)
                    db.Data[y * db.Width + x] = srcBitmap.GetPixel(x, y).ToArgb();

            if (log)
                Console.WriteLine("Loaded DB in " + (DateTime.Now - start).TotalSeconds + " seconds");

            return db;
        }

        public void SetPixel(int x, int y, Color color) => Write(x, y, color.ToArgb());

        public void SetPixelChecked(int x, int y, Color color)
        {
            if (x > -1 && x < Width && y > -1 && y < Height)
                Write(x, y, color.ToArgb());
        }

        public Color GetPixel(int x, int y) => Color.FromArgb(Read<int>(x, y));

        public void Clear(Color color) => Clear(color.ToArgb());

        object accessLock = new object();

        public Color SampleColour(float x, float y)
        {
            int sx;
            int sy;
            lock (accessLock)
            {
                sx = (int)(x * (float)Width);
                sy = (int)(y * (float)Height - 1.0f);
            }
            if (sx < 0 || sx >= Width || sy < 0 || sy >= Height)
                return Color.Black;
            else
                lock (accessLock)
                {
                    return Color.FromArgb(Read<int>(sy * Width + sx));
                }
        }

        public void CopyTo(DirectBitmap destination) => destination.Write(0, Data);

        #endregion
    }



    public static class BmEx
    {
        public static Color SampleColour(this Bitmap bm, float x, float y)
        {
            //lock (writeLock)
            //{
            int sx = (int)(x * (float)bm.Width);
            int sy = (int)(y * (float)bm.Height - 1.0f);
            if (sx < 0 || sx >= bm.Width || sy < 0 || sy >= bm.Height)
                return Color.Black;
            else
                return bm.GetPixel(sx, sy);//Color.FromArgb(Read<int>(sy * Width + sx));
            //}
        }

        public static void DrawPlus(this DirectBitmap to, DirectBitmap from, int x, int y)
        {
            Parallel.For(0, from.Height, (int i) =>
            {
                to.Write((y + i) * to.Width + x, from.Data, i * from.Width, from.Width);
            });
        }

        public static void Draw(this DirectBitmap to, Bitmap from, int x, int y)
        {
            for (int j = 0; j < from.Height; j++)
            {
                for (int i = 0; i < from.Width; i++)
                {
                    var destOfs = (x + i) + (y + j) * to.Width;
                    to.Write<int>(destOfs, from.GetPixel(i, j).ToArgb());
                }
            }
        }

        internal static void Draw(this DirectBitmap to, Font.GlyphBitmap from, int x, int y, int fc, int bc)
        {
            for (int j = 0; j < from.Height; j++)
            {
                for (int i = 0; i < from.Width; i++)
                {
                    //int destOfs = x + i + (y + j) * to.Width;
                    //int fromOfs = i + j * from.Width;
                    if (from.Pixels[i + j * from.Width] > 1)
                        to.Write<int>(x + i + (y + j) * to.Width, fc);
                    else
                        to.Write<int>(x + i + (y + j) * to.Width, bc);
                }
            }
        }

        public static void Draw(this DirectBitmap to, DirectBitmap from, int x, int y)
        {
            Parallel.For(0, from.Height, (int j) =>
            {
                Parallel.For(0, from.Width, (int i) =>
                {
                    int destOfs = (x + i) + (y + j) * to.Width;
                    int fromOfs = i + j * from.Width;
                    to.Write<int>(destOfs, from.Read<int>(fromOfs));
                });
            });
        }
        public static void Drawx2(this DirectBitmap to, DirectBitmap from, int x, int y)
        {
            Parallel.For(0, from.Height, (int Y) =>
            {
                Parallel.For(0, from.Width, (int X) =>
                {
                    int destOfs = (x + X * 2) + (y + Y * 2) * to.Width;
                    int fromOfs = X + Y * from.Width;
                    int clr = from.Read<int>(fromOfs);

                    to.Write<int>(destOfs, clr);
                    to.Write<int>(destOfs + 1, clr);
                    to.Write<int>(destOfs + to.Width, clr);
                    to.Write<int>(destOfs + 1 + to.Width, clr);
                });
            });

            /*for (int X = 0; X < from.Width; X++)
                for (int Y = 0; Y < from.Height; Y++)
                {
                    int destOfs = (x + X * 2) + (y + Y * 2) * to.Width;
                    int fromOfs = X + Y * from.Width;
                    int clr = from.Read<int>(fromOfs);

                    to.Write<int>(destOfs, clr);
                    to.Write<int>(destOfs + 1, clr);
                    to.Write<int>(destOfs + to.Width, clr);
                    to.Write<int>(destOfs + 1 + to.Width, clr);
                }*/
        }
    }
}
