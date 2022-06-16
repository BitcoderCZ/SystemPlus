using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SystemPlus.UI;
using SystemPlus.Utils;

namespace SystemPlus.Extensions
{
    public static class AdvencedConsole
    {
        private static SafeFileHandle h;

        static AdvencedConsole()
        {
            h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        }

        public static bool SetBuffer(CharInfo[] buffer, short width, short height)
        {
            if (h.IsInvalid)
                throw new Exception("Handle is invalid");
            else if (h.IsClosed)
                throw new Exception("Handle is closed");

            SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = width, Bottom = height };
            return WriteConsoleOutputW(h, buffer,
                                       new Coord() { X = width, Y = height },
                                       new Coord() { X = 0, Y = 0 },
                                       ref rect);
        }

        public static bool SetBuffer(ConsoleColor[] buffer, short width, short height)
        {
            CharInfo[] charBuffer = new CharInfo[buffer.Length * 2];
            Parallel.For(0, buffer.Length, (int i) =>
            {
                CharInfo inf = new CharInfo { Attributes = (short)((short)buffer[i] << 4) };
                inf.Char.AsciiChar = (byte)' ';
                charBuffer[i * 2] = inf;
                charBuffer[i * 2 + 1] = inf;
            });
            return SetBuffer(charBuffer, (short)(width * 2), height);
        }
        public static bool SetBuffer(Color[] buffer, short width, short height)
        {
            CharInfo[] charBuffer = new CharInfo[buffer.Length * 2];
            Parallel.For(0, buffer.Length, (int i) =>
            {
                CharInfo inf = new CharInfo { Attributes = (short)((short)buffer[i].ToConsoleColor() << 4) };
                inf.Char.AsciiChar = (byte)' ';
                charBuffer[i * 2] = inf;
                charBuffer[i * 2 + 1] = inf;
            });
            return SetBuffer(charBuffer, (short)(width * 2), height);
        }
        public static bool SetBuffer(int[] buffer, short width, short height)
        {
            CharInfo[] charBuffer = new CharInfo[buffer.Length * 2];
            Parallel.For(0, buffer.Length, (int i) =>
            {
                CharInfo inf = new CharInfo { Attributes = (short)((short)Color.FromARGB(buffer[i]).ToConsoleColor() << 4) };
                inf.Char.AsciiChar = (byte)' ';
                charBuffer[i * 2] = inf;
                charBuffer[i * 2 + 1] = inf;
            });
            return SetBuffer(charBuffer, (short)(width * 2), height);
        }
        public static bool SetBuffer(DirectBitmap buffer)
        {
            CharInfo[] charBuffer = new CharInfo[buffer.Data.Length * 2];
            Parallel.For(0, buffer.Data.Length, (int i) =>
            {
                CharInfo inf = new CharInfo { Attributes = (short)((short)Color.FromARGB(buffer[i]).ToConsoleColor() << 4) };
                inf.Char.AsciiChar = (byte)' ';
                charBuffer[i * 2] = inf;
                charBuffer[i * 2 + 1] = inf;
            });
            return SetBuffer(charBuffer, (short)(buffer.Width * 2), (short)buffer.Height);
        }
        public static bool SetBuffer(Image buffer)
            => SetBuffer(buffer.pixels, (short)buffer.Width, (short)buffer.Height);
        public static bool SetBuffer(ConsoleImage buffer)
            => SetBuffer(buffer.Data.ToNewType((byte b) => (ConsoleColor)b), buffer.Width, buffer.Height);

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
        static extern bool WriteConsoleOutputW(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputAttribute(
          SafeFileHandle hConsoleOutput,
          int[] lpAttribute,
          uint nLength,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
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
            [FieldOffset(0)] public ushort UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}
