using System;
using System.Collections.Generic;
using SystemPlus.Extensions;

namespace SystemPlus
{
    public class ColoredChar
    {
        public char ch;
        public ConsoleColor fgC;
        public ConsoleColor bgC;

        public ColoredChar(char _char, ConsoleColor _fgC, ConsoleColor _bgC)
        {
            ch = _char;
            fgC = _fgC;
            bgC = _bgC;
        }

        public void Print()
        {
            ConsoleColor forg = Console.ForegroundColor;
            ConsoleColor back = Console.BackgroundColor;
            Console.ForegroundColor = fgC;
            Console.BackgroundColor = bgC;
            Console.Write(ch);
            Console.ForegroundColor = forg;
            Console.BackgroundColor = back;
        }

        public string ToSavableString()
        {
            string fgc = (((int)fgC).ToString().Length < 10) ? $" {(int)fgC}" : ((int)fgC).ToString();
            string bgc = (((int)bgC).ToString().Length < 10) ? $" {(int)bgC}" : ((int)bgC).ToString();

            return $"{ch}{fgc}{bgc}";
        }

        public static explicit operator ColoredChar(char ch)
            => new ColoredChar(ch, ConsoleColor.White, ConsoleColor.Black);
        public static implicit operator char(ColoredChar cCh)
            => cCh.ch;
    }

    public class ColoredString
    {
        private List<ColoredChar> chars;

        public int Length => chars.Count;

        public ColoredString(string s, char colorIndicator = '#')
        {
            chars = new List<ColoredChar>();
            /*for (int i = 0; i < s.Length; i++)
                chars.Add(new ColoredChar(' ', ConsoleColor.Black, ConsoleColor.White));*/

            ConsoleColor lastFg = ConsoleColor.White;
            ConsoleColor lastBg = ConsoleColor.Black;
            for (int i = 0; i < s.Length; i++)
            {
                try
                {
                    if (i >= s.Length)
                        return;
                    if (s[i] == colorIndicator)
                    {
                        ConsoleColor fgc;
                        if (s[i + 2] != '0')
                            fgc = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), int.Parse(s[i + 2].ToString() + s[i + 3].ToString()).ToString());
                        else
                            fgc = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), int.Parse(s[i + 3].ToString()).ToString());
                        ConsoleColor bgc;
                        if (s[i + 4] != '0')
                            bgc = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), int.Parse(s[i + 4].ToString() + s[i + 5].ToString()).ToString());
                        else
                            bgc = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), int.Parse(s[i + 5].ToString()).ToString());
                        chars.Add(new ColoredChar(s[i + 1], lastFg, lastBg));//[index] = new ColoredChar(s[i + 1], lastFg, lastBg);

                        lastFg = fgc;
                        lastBg = bgc;

                        i += 6;

                    }
                    else
                        chars.Add(new ColoredChar(s[i], lastFg, lastBg));//[index] = new ColoredChar(s[i], lastFg, lastBg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                int jj = 0;
            }

            int t = Console.CursorTop;
            int l = Console.CursorLeft;
            Console.SetCursorPosition(5, 40);
            Console.WriteLine(s.Length);
            Console.WriteLine(chars.Count);
            Console.CursorTop = t;
            Console.CursorLeft = l;
        }

        public void Print()
        {
            if (chars.Count == 0)
                return;

            Console.ForegroundColor = chars[0].fgC;
            Console.BackgroundColor = chars[0].bgC;

            Console.Write(chars[0].ch);

            for (int i = 1; i < chars.Count; i++)
            {
                if (chars[i].ch == '\n')
                {
                    Console.CursorTop++;
                    Console.CursorLeft = 0;
                }
                else if (chars[i].fgC == Console.ForegroundColor && chars[i].bgC == Console.BackgroundColor)
                    Console.Write(chars[i].ch);
                else
                {
                    Console.ForegroundColor = chars[i].fgC;
                    Console.BackgroundColor = chars[i].bgC;
                    Console.Write(chars[i].ch);
                }
            }
            Console.ResetColor();
        }

        public override string ToString()
        {
            string s = "";
            chars.ForEach((ColoredChar ch) => { s += ch.ch; });
            return s;
        }
        public char[] ToArray() => chars.ToArray().ChangeType(CCH => CCH.ch);
        public ColoredChar[] ToColoredArray() => chars.ToArray();
        public string ToSavableString(char colorIndicator = '#')
        {
            if (chars.Count == 0)
                return null;

            string s = "";
            ConsoleColor lfgc = chars[0].fgC;
            ConsoleColor lbgc = chars[0].bgC;

            s += colorIndicator + chars[0].ToSavableString();

            for (int i = 1; i < chars.Count; i++)
            {
                if (chars[i].fgC == lfgc && chars[i].bgC == lbgc)
                    s += chars[i].ch;
                else
                {
                    s += colorIndicator + chars[i].ToSavableString();
                    lfgc = chars[i].fgC;
                    lbgc = chars[i].bgC;
                }
            }

            return s;
        }

        public static implicit operator ColoredString(string s)
            => new ColoredString(s);
        public static explicit operator string(ColoredString s)
            => s.ToString();
    }
}
