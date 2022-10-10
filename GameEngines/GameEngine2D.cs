using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SystemPlus.Extensions;
using SystemPlus.Vectors;

namespace SystemPlus.GameEngines
{
    public enum CONSOLE_COLOR
    {
        Black = 0,
        DarkBlue = 1,
        DarkGreen = 2,
        DarkCyan = 3,
        DarkRed = 4,
        DarkMagenta = 5,
        DarkYellow = 6,
        Gray = 7,
        DarkGray = 8,
        Blue = 9,
        Green = 10,
        Cyan = 11,
        Red = 12,
        Magenta = 13,
        Yellow = 14,
        White = 15
    }

    public enum PIXEL
    {
        PIXEL_QUARTER = 176,//'█',0x2588,
        PIXEL_HALF = 177,//'▓',0x2593,
        PIXEL_THREEQUARTERS = 178,//'▒',0x2592,
        PIXEL_SOLID = 219,//'░',0x2591,
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct Coord
    {
        public short X;
        public short Y;

        public Coord(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct CharUnion
    {
        [FieldOffset(0)] public char UnicodeChar;
        [FieldOffset(0)] public byte AsciiChar;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
    public abstract class GameEngine2D
    {
        public float FPS;
        public int MaxFPS = 30;
        private Vector2Int bufferSize;
        private string title;
        private CharInfo[] buffer;
        SmallRect rect;
        private bool PixOneByOne;

        public bool Clear = true;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        public abstract StartInfo2D Start();
        public abstract void Update(float deltaTime, int miliseconds);
        int pw = 3;
        public GameEngine2D()
        {
            StartInfo2D info = Start();
            PixOneByOne = info.pixelsOneByOne;
            if (PixOneByOne)
            {
                bufferSize = new Vector2Int(info.Width * pw, info.Height);
                buffer = new CharInfo[(info.Width * pw) * info.Height];
                rect = new SmallRect() { Left = 0, Top = 0, Right = (short)(info.Width * pw), Bottom = info.Height };
            }
            else
            {
                bufferSize = new Vector2Int(info.Width, info.Height);
                buffer = new CharInfo[info.Width * info.Height];
                rect = new SmallRect() { Left = 0, Top = 0, Right = info.Width, Bottom = info.Height };
            }
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (h.IsInvalid)
                throw new Exception("ERROR");
            ConsoleExtensions.SetFontSize(info.pixelSize);
            Thread.Sleep(500);
            GeneralExtensions.MaxScreen();
            title = info.Title;
            FPS = 10f;
            Console.Title = title + " - FPS: " + FPS;
            var gameLoopThread = new Thread(() => GameLoop(rect, h));
            gameLoopThread.Start();
        }

        private void GameLoop(SmallRect rect, SafeFileHandle h)
        {
            int milis = 10;
            float deltaTime = 0.01f;

            while (true)
            {
                DateTime frameStart = DateTime.Now;

                if (Clear)
                    buffer = new CharInfo[bufferSize.x * bufferSize.y];

                Update(deltaTime, milis);

                bool b = WriteConsoleOutput(h, buffer,
                new Coord() { X = (short)bufferSize.x, Y = (short)bufferSize.y },
                new Coord() { X = 0, Y = 0 },
                ref rect);

                TimeSpan fl = DateTime.Now - frameStart;
                if (1000f / (float)MaxFPS > fl.Milliseconds)
                    Thread.Sleep((int)(1000f / (float)MaxFPS - fl.Milliseconds));

                TimeSpan span = DateTime.Now - frameStart;
                milis = (int)(span.Ticks / TimeSpan.TicksPerMillisecond);
                deltaTime = (float)milis / 1000f;
                FPS = 1000f / (float)milis;

                Console.Title = title + " - FPS: " + MathPlus.Round(FPS * 100f) / 100f;

                Thread.Sleep(0);
            }
        }

        public bool Draw(short x, short y, short pixelType = 0x2588, short color = 0x000F)
        {
            if (!PixOneByOne)
            {
                if (x >= 0 && x < bufferSize.x && y >= 0 && y < bufferSize.y)
                {
                    buffer[y * bufferSize.x + x].Char.UnicodeChar = (char)pixelType;
                    buffer[y * bufferSize.x + x].Attributes = color;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (x >= 0 && x < (bufferSize.x / pw) - 1 && y >= 0 && y < bufferSize.y)
                {
                    x *= (short)pw;
                    buffer[y * bufferSize.x + x].Char.UnicodeChar = (char)pixelType;
                    buffer[y * bufferSize.x + x].Attributes = color;
                    buffer[y * bufferSize.x + x + 1].Char.UnicodeChar = (char)pixelType;
                    buffer[y * bufferSize.x + x + 1].Attributes = color;
                    buffer[y * bufferSize.x + x + 2].Char.UnicodeChar = (char)pixelType;
                    buffer[y * bufferSize.x + x + 2].Attributes = color;
                    return true;
                }
                else
                    return false;
            }
        }
        public bool Draw(short x, short y, PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => Draw(x, y, (short)pixelType, (short)color);
        public bool Draw(Vector2Short pos, short pixelType = 0x2588, short color = 0x000F)
            => Draw(pos.x, pos.y, pixelType, color);
        public bool Draw(Vector2Short pos, PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => Draw(pos, (short)pixelType, (short)(color));


        public void Fill(int x1, int y1, int x2, int y2, short pixelType = 0x2588, short color = 0x000F)
        {
            Clip(ref x1, ref y1);
            Clip(ref x2, ref y2);
            /*Parallel.For(x1, x2, (int x) =>
            {
                Parallel.For(y1, y2, (int y) =>
                {
                    Draw((short)x, (short)y, pixelType, color);
                });
            });*/
            for (int x = x1; x < x2; x++)
                for (int y = y1; y < y2; y++)
                    Draw((short)x, (short)y, pixelType, color);
        }
        public void Fill(int x1, int y1, int x2, int y2,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => Fill(x1, y1, x2, y2, (short)pixelType, (short)color);
        public void Fill(Vector2Short pos1, Vector2Short pos2, short pixelType = 0x2588, short color = 0x000F)
            => Fill(pos1.x, pos1.y, pos2.x, pos2.y, pixelType, color);
        public void Fill(Vector2Short pos1, Vector2Short pos2,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => Fill(pos1, pos2, (short)pixelType, (short)color);


        public void DrawString(int x, int y, string s, short color = 0x000F)
        {
            for (int i = 0; i < s.ToCharArray().Length; i++)
            {
                buffer[y * bufferSize.x + x + i].Char.UnicodeChar = s.ToCharArray()[i];
                buffer[y * bufferSize.x + x + i].Attributes = color;
            }
        }
        public void DrawString(int x, int y, string s, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawString(x, y, s, (short)color);
        public void DrawString(Vector2Short pos, string s, short color = 0x000F)
            => DrawString(pos.x, pos.y, s, color);
        public void DrawString(Vector2Short pos, string s, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawString(pos, s, (short)color);


        public void Clip(ref int x, ref int y)
        {
            if (x < 0) x = 0;
            if (x >= bufferSize.x) x = bufferSize.x;
            if (y < 0) y = 0;
            if (y >= bufferSize.y) y = bufferSize.y;
        }


        public void DrawLine(int x1, int y1, int x2, int y2, short pixelType = 0x2588, short color = 0x000F)
        {
            int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            dx = x2 - x1; dy = y2 - y1;
            dx1 = Math.Abs(dx); dy1 = Math.Abs(dy);
            px = 2 * dy1 - dx1; py = 2 * dx1 - dy1;
            if (dy1 <= dx1)
            {
                if (dx >= 0)
                { x = x1; y = y1; xe = x2; }
                else
                { x = x2; y = y2; xe = x1; }

                Draw((short)x, (short)y, pixelType, color);

                for (i = 0; x < xe; i++)
                {
                    x = x + 1;
                    if (px < 0)
                        px = px + 2 * dy1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) y = y + 1; else y = y - 1;
                        px = px + 2 * (dy1 - dx1);
                    }
                    Draw((short)x, (short)y, pixelType, color);
                }
            }
            else
            {
                if (dy >= 0)
                { x = x1; y = y1; ye = y2; }
                else
                { x = x2; y = y2; ye = y1; }

                Draw((short)x, (short)y, pixelType, color);

                for (i = 0; y < ye; i++)
                {
                    y = y + 1;
                    if (py <= 0)
                        py = py + 2 * dx1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) x = x + 1; else x = x - 1;
                        py = py + 2 * (dx1 - dy1);
                    }
                    Draw((short)x, (short)y, pixelType, color);
                }
            }
        }
        public void DrawLine(int x1, int y1, int x2, int y2,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawLine(x1, y1, x2, y2, (short)pixelType, (short)color);
        public void DrawLine(Vector2Short pos1, Vector2Short pos2, short pixelType = 0x2588, short color = 0x000F)
            => DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, pixelType, color);
        public void DrawLine(Vector2Short pos1, Vector2Short pos2,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawLine(pos1, pos2, (short)pixelType, (short)color);


        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3,
            short pixelType = 0x2588, short color = 0x000F)
        {
            DrawLine(x1, y1, x2, y2, pixelType, color);
            DrawLine(x2, y2, x3, y3, pixelType, color);
            DrawLine(x3, y3, x1, y1, pixelType, color);
        }
        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawTriangle(x1, y1, x2, y2, x3, y3, (short)pixelType, (short)color);
        public void DrawTriangle(Vector2Short pos1, Vector2Short pos2, Vector2Short pos3,
            short pixelType = 0x2588, short color = 0x000F)
            => DrawTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, pixelType, color);
        public void DrawTriangle(Vector2Short pos1, Vector2Short pos2, Vector2Short pos3,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawTriangle(pos1, pos2, pos3, (short)pixelType, (short)color);


        private void SWAP(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }
        private void drawLine(int sx, int ex, int ny, short pixelType, short color)
        {
            for (int i = sx; i <= ex; i++)
                Draw((short)i, (short)ny, pixelType, color);
        }


        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3,
            short pixelType = 0x2588, short color = 0x000F)
        {
            int t1x, t2x, y, minx, maxx, t1xp, t2xp;
            bool changed1 = false;
            bool changed2 = false;
            int signx1, signx2, dx1, dy1, dx2, dy2;
            int e1, e2;
            // Sort vertices
            if (y1 > y2) { SWAP(ref y1, ref y2); SWAP(ref x1, ref x2); }
            if (y1 > y3) { SWAP(ref y1, ref y3); SWAP(ref x1, ref x3); }
            if (y2 > y3) { SWAP(ref y2, ref y3); SWAP(ref x2, ref x3); }

            t1x = t2x = x1; y = y1;   // Starting points
            dx1 = (int)(x2 - x1); if (dx1 < 0) { dx1 = -dx1; signx1 = -1; }
            else signx1 = 1;
            dy1 = (int)(y2 - y1);

            dx2 = (int)(x3 - x1); if (dx2 < 0) { dx2 = -dx2; signx2 = -1; }
            else signx2 = 1;
            dy2 = (int)(y3 - y1);

            if (dy1 > dx1)
            {   // swap values
                SWAP(ref dx1, ref dy1);
                changed1 = true;
            }
            if (dy2 > dx2)
            {   // swap values
                SWAP(ref dy2, ref dx2);
                changed2 = true;
            }

            e2 = (int)(dx2 >> 1);
            // Flat top, just process the second half
            if (y1 == y2) goto next;
            e1 = (int)(dx1 >> 1);

            for (int i = 0; i < dx1;)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    i++;
                    e1 += dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= dx1;
                        if (changed1) t1xp = signx1;//t1x += signx1;
                        else goto next1;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                }
            // Move line
            next1:
                // process second line until y value is about to change
                while (true)
                {
                    e2 += dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= dx2;
                        if (changed2) t2xp = signx2;//t2x += signx2;
                        else goto next2;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
            next2:
                if (minx > t1x) minx = t1x; if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x; if (maxx < t2x) maxx = t2x;
                drawLine(minx, maxx, y, pixelType, color);    // Draw line from min to max points found on the y
                                                              // Now increase y
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y == y2) break;

            }
        next:
            // Second half
            dx1 = (int)(x3 - x2); if (dx1 < 0) { dx1 = -dx1; signx1 = -1; }
            else signx1 = 1;
            dy1 = (int)(y3 - y2);
            t1x = x2;

            if (dy1 > dx1)
            {   // swap values
                SWAP(ref dy1, ref dx1);
                changed1 = true;
            }
            else changed1 = false;

            e1 = (int)(dx1 >> 1);

            for (int i = 0; i <= dx1; i++)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    e1 += dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= dx1;
                        if (changed1) { t1xp = signx1; break; }//t1x += signx1;
                        else goto next3;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                    if (i < dx1) i++;
                }
            next3:
                // process second line until y value is about to change
                while (t2x != x3)
                {
                    e2 += dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= dx2;
                        if (changed2) t2xp = signx2;
                        else goto next4;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
            next4:

                if (minx > t1x) minx = t1x; if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x; if (maxx < t2x) maxx = t2x;
                drawLine(minx, maxx, y, pixelType, color);
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y > y3) return;
            }
        }
        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => FillTriangle(x1, y1, x2, y2, x3, y3, (short)pixelType, (short)color);
        public void FillTriangle(Vector2Short pos1, Vector2Short pos2, Vector2Short pos3,
            short pixelType = 0x2588, short color = 0x000F)
            => FillTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, pixelType, color);
        public void FillTriangle(Vector2Short pos1, Vector2Short pos2, Vector2Short pos3,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => FillTriangle(pos1, pos2, pos3, (short)pixelType, (short)color);


        public void DrawCircle(int xc, int yc, int radius, short pixelType = 0x2588, short color = 0x000F)
        {
            int x = 0;
            int y = radius;
            int p = 3 - 2 * radius;
            if (radius <= 0) return;

            while (y >= x) // only formulate 1/8 of circle
            {
                Draw((short)(xc - x), (short)(yc - y), pixelType, color);//upper left left
                Draw((short)(xc - y), (short)(yc - x), pixelType, color);//upper upper left
                Draw((short)(xc + y), (short)(yc - x), pixelType, color);//upper upper right
                Draw((short)(xc + x), (short)(yc - y), pixelType, color);//upper right right
                Draw((short)(xc - x), (short)(yc + y), pixelType, color);//lower left left
                Draw((short)(xc - y), (short)(yc + x), pixelType, color);//lower lower left
                Draw((short)(xc + y), (short)(yc + x), pixelType, color);//lower lower right
                Draw((short)(xc + x), (short)(yc + y), pixelType, color);//lower right right
                if (p < 0) p += 4 * x++ + 6;
                else p += 4 * (x++ - y--) + 10;
            }
        }
        public void DrawCircle(int xc, int yc, int radius,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawCircle(xc, yc, radius, (short)pixelType, (short)color);
        public void DrawCircle(Vector2Short pos, int radius, short pixelType = 0x2588, short color = 0x000F)
            => DrawCircle(pos.x, pos.y, radius, pixelType, color);
        public void DrawCircle(Vector2Short pos, int radius,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => DrawCircle(pos, radius, (short)pixelType, (short)color);


        public void FillCircle(int xc, int yc, int radius, short pixelType = 0x2588, short color = 0x000F)
        {
            // Taken from wikipedia
            int x = 0;
            int y = radius;
            int p = 3 - 2 * radius;
            if (radius <= 0) return;

            while (y >= x)
            {
                // Modified to draw scan-lines instead of edges
                drawLine(xc - x, xc + x, yc - y, pixelType, color);
                drawLine(xc - y, xc + y, yc - x, pixelType, color);
                drawLine(xc - x, xc + x, yc + y, pixelType, color);
                drawLine(xc - y, xc + y, yc + x, pixelType, color);
                if (p < 0) p += 4 * x++ + 6;
                else p += 4 * (x++ - y--) + 10;
            }
        }
        public void FillCircle(int xc, int yc, int radius,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => FillCircle(xc, yc, radius, (short)pixelType, (short)color);
        public void FillCircle(Vector2Short pos, int radius, short pixelType = 0x2588, short color = 0x000F)
            => FillCircle(pos.x, pos.y, radius, pixelType, color);
        public void FillCircle(Vector2Short pos, int radius,
            PIXEL pixelType = PIXEL.PIXEL_SOLID, CONSOLE_COLOR color = CONSOLE_COLOR.White)
            => FillCircle(pos, radius, (short)pixelType, (short)color);


        public short CreateClr(CONSOLE_COLOR foregroundcoloror, CONSOLE_COLOR backgroundcoloror) => (short)((int)foregroundcoloror | ((int)backgroundcoloror << 4));
    }

    public class StartInfo2D
    {
        public string Title;
        public readonly short Width;
        public readonly short Height;
        public readonly short pixelSize;
        public readonly bool pixelsOneByOne;
        public StartInfo2D(string _title, short _width, short _height, short _fontSize, bool _pixelsOneByOne = true)
        {
            Title = _title;
            if (_width <= 0)
                throw new Exception("Width is less than 1");
            if (_height <= 0)
                throw new Exception("Height is less than 1");
            if (_fontSize <= 0)
                throw new Exception("Font size is less than 1");
            Width = _width;
            Height = _height;
            pixelSize = _fontSize;
            pixelsOneByOne = _pixelsOneByOne;
        }
    }
}
