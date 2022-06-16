using System;
using System.Collections.Generic;
using SystemPlus.UI;
using SystemPlus.Vectors;

namespace SystemPlus.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        ///  Returns index of closetst color
        /// </summary>
        public static int ClosestColor(this Color c, Color[] colors)
        {
            List<Vector4> closness = new List<Vector4>();
            for (int i = 0; i < colors.Length; i++)
            {
                Color cr = colors[i];
                int RD = (c.R - cr.R < 0) ? -(c.R - cr.R) : c.R - cr.R;
                int GD = (c.G - cr.G < 0) ? -(c.G - cr.G) : c.G - cr.G;
                int BD = (c.B - cr.B < 0) ? -(c.B - cr.B) : c.B - cr.B;
                closness.Add(new Vector4(RD, GD, BD, i));
            }

            List<Vector2> combinedClosness = new List<Vector2>();

            foreach (Vector4 v4 in closness)
                combinedClosness.Add(new Vector2(v4.x + v4.y + v4.z, v4.w));

            int index = -1;
            int score = 765;
            for (int i = 0; i < combinedClosness.Count; i++)
                if (combinedClosness[i].x < score)
                {
                    score = (int)combinedClosness[i].x;
                    index = (int)combinedClosness[i].y;
                }
            return index;
        }

        public static ConsoleColor ClosestConsoleColor(Color clr)
        {
            ConsoleColor ret = 0;
            double rr = clr.R, gg = clr.G, bb = clr.B, delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta)
                {
                    delta = t;
                    ret = cc;
                }
            }
            return ret;
        }

        public static Color FromConsoleColor(ConsoleColor c)
        {
            int[] cColors = {   0x000000, //Black = 0
                        0x000080, //DarkBlue = 1
                        0x008000, //DarkGreen = 2
                        0x008080, //DarkCyan = 3
                        0x800000, //DarkRed = 4
                        0x800080, //DarkMagenta = 5
                        0x808000, //DarkYellow = 6
                        0xC0C0C0, //Gray = 7
                        0x808080, //DarkGray = 8
                        0x0000FF, //Blue = 9
                        0x00FF00, //Green = 10
                        0x00FFFF, //Cyan = 11
                        0xFF0000, //Red = 12
                        0xFF00FF, //Magenta = 13
                        0xFFFF00, //Yellow = 14
                        0xFFFFFF  //White = 15
                    };
            return Color.FromARGB(cColors[(int)c]);
        }
    }
}
