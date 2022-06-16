using System;
using SystemPlus.Extensions;

namespace SystemPlus.UI
{
    //[StructLayout(LayoutKind.Sequential)]
    public unsafe struct Color
    {
        byte r;
        byte g;
        byte b;
        byte a;

        public byte R => r;
        public byte G => g;
        public byte B => b;
        public byte A => a;

        private Color(byte _A, byte _R, byte _G, byte _B)
        {
            r = _R;
            g = _G;
            b = _B;
            a = _A;
        }

        public static Color From8(byte value)
        {
            int index = value & 15;
            int _alpha = value & 240;
            int alpha;
            switch (_alpha)
            {
                case 0:
                    alpha = 64;
                    break;
                case 1:
                    alpha = 127;
                    break;
                case 2:
                    alpha = 191;
                    break;
                case 3:
                    alpha = 255;
                    break;
                default:
                    alpha = 255;
                    break;
            }

            Color color = ColorExtensions.FromConsoleColor((ConsoleColor)index);//BasicColors[index];
            color.a = (byte)alpha;
            return color;
        }
        public static Color FromARGB(Int32 value)
        {
            Int32 __B = value & 255;
            Int32 __G = value & 65280;
            Int32 __R = value & 16711680;
            Int32 __A = (Int32)((long)value & 4278190080);
            UInt32 _B = (UInt32)__B;
            UInt32 _G = (UInt32)__G >> 8;
            UInt32 _R = (UInt32)__R >> 16;
            UInt32 _A = (UInt32)__A >> 24;
            return new Color((byte)_A, (byte)_R, (byte)_G, (byte)_B);
        }
        public static Color FromARGB(byte _R, byte _G, byte _B)
            => new Color(_R, _G, _B, 255);
        public static Color FromARGB(byte _A, byte _R, byte _G, byte _B)
            => new Color(_A, _R, _G, _B);

        public static Color[] BasicColors = new Color[]
        {
            new Color(255, 0, 0, 0), // black
            new Color(255, 0, 0, 139), // dark blue
            new Color(255, 0, 139, 0), // dark green
            new Color(255, 0, 139, 139), // dark cyan
            new Color(255, 139, 0, 0), // dark red
            new Color(255, 139, 0, 139), // dark magenta
            new Color(255, 139, 139, 0), // dark yellow
            new Color(255, 168, 168, 168), // gray
            new Color(255, 128, 128, 128), // dark gray
            new Color(255, 0, 0, 255), // blue
            new Color(255, 0, 0, 255), // green
            new Color(255, 0, 255, 255), // cyan
            new Color(255, 255, 0, 0), // red
            new Color(255, 255, 0, 255), // magenta
            new Color(255, 255, 255, 0), // yellow
            new Color(255, 255, 255, 255), // white
        };

        public byte To8()
        {
            byte color = (byte)((int)ColorExtensions.ClosestConsoleColor(this));//(byte)this.ClosestColor(BasicColors);
            byte alpha;

            if (a <= 64)
                alpha = 0;
            else if (a <= 127)
                alpha = 1;
            else if (a <= 191)
                alpha = 2;
            else
                alpha = 3;

            alpha = (byte)((int)alpha << 4);
            byte value = 0;
            value = (byte)((int)value | (int)color);
            value = (byte)((int)value | (int)alpha);
            return value;
        }
        public ConsoleColor ToConsoleColor() // (short)((int)foregroundColor | ((int)backgroundColor << 4))
        {
            return (ConsoleColor)this.ClosestColor(BasicColors);
        }
        public Int32 To32()
        {
            int value = 0;
            int _A = a << 24;
            int _R = r << 16;
            int _G = g << 8;
            int _B = b;

            value = value | _A;
            value = value | _R;
            value = value | _G;
            value = value | _B;
            return (Int32)value;
        }

        public static Int32 To32(byte a, byte r, byte g, byte b)
        {
            int value = 0;
            int _A = a << 24;
            int _R = r << 16;
            int _G = g << 8;
            int _B = b;

            value = value | _A;
            value = value | _R;
            value = value | _G;
            value = value | _B;
            return (Int32)value;
        }

        public static explicit operator System.Drawing.Color(Color c)
            => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);

        public static explicit operator Color(System.Drawing.Color c)
            => FromARGB(c.A, c.R, c.G, c.B);

        public override string ToString()
        {
            return $"A: {a}, R: {r}, G: {g}, B: {b}";
        }
    }
}
