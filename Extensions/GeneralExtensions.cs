using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SystemPlus.Vectors;

namespace SystemPlus.Extensions
{
    public enum MergeMode
    {
        OneToOne,
        TwoToOne,
        ThreeToOne,
        FourToOne,
        FiveToOne,
    }
    public static class GeneralExtensions
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        public static void MaxScreen()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }

        public static void Wait(int milis)
        {
            Thread.Sleep(milis);
        }

        public static IntPtr Handle(this System.Windows.Forms.Control window)
            => window.IsDisposed ? default : Handle((System.Windows.Forms.IWin32Window)window);

        public static IntPtr Handle(this System.Windows.Forms.IWin32Window window) => window.Handle;

        [STAThread]
        public static IntPtr Handle(this System.Windows.Media.Visual window)
        {
            System.Windows.Interop.HwndSource handleSource = window.HandleSource();
            return handleSource == null || handleSource.IsDisposed ? default : handleSource.Handle;
        }

        [STAThread]
        public static System.Windows.Interop.HwndSource HandleSource(this System.Windows.Media.Visual window)
            => System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;

        #region conversions

        public static Vector2D ToVector2D(this System.Drawing.Point p) => new Vector2D(p.X, p.Y);
        public static Vector2D ToVector2D(this System.Windows.Point p) => new Vector2D(p.X, p.Y);

        public static Vector2Int ToVector2Int(this Vector2D p) => new Vector2Int((int)p.X, (int)p.Y);

        #endregion

        public static string ToBits(this int a) => Convert.ToString(a, toBase: 2);
        public static string ToBits(this uint a) => Convert.ToString(a, toBase: 2);
        public static string ToBits(this long a) => Convert.ToString(a, toBase: 2);

        public static Int32 ReadInt(this FileStream stream)
        {
            try
            {
                byte[] bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                    bytes[i] = (byte)stream.ReadByte();
                return BitConverter.ToInt32(bytes, 0);
            }
            catch { return -1; }
        }

        public static Int32 ReadInt(this byte[] bytes, ref int index)
        {
            int value = 0;
            int a = bytes[index + 3] << 24;
            int b = bytes[index + 2] << 16;
            int c = bytes[index + 1] << 8;
            int d = bytes[index];

            value = value | a;
            value = value | b;
            value = value | c;
            value = value | d;

            index += 4;

            return value;
        }

        public static UI.Color ReadColor(this byte[] bytes, ref int index) => UI.Color.FromARGB(bytes.ReadInt(ref index));

        /// <param name="filter">(Image|*.png;*.jpg, Text|*.txt)</param>
        /// <returns>results (if error returns null)</returns>
        public static string[] ShowFileDialog(string title, string filter, bool multiselect)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = multiselect,
                Title = title,
                Filter = filter,
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    return dialog.FileNames;
                else
                    return null;
            }
        }

        public static int GetDiffrence(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length; // length of s
            int m = t.Length; // length of t

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int[] p = new int[n + 1]; //'previous' cost array, horizontally
            int[] d = new int[n + 1]; // cost array, horizontally

            // indexes into strings s and t
            int i; // iterates through s
            int j; // iterates through t

            for (i = 0; i <= n; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= m; j++)
            {
                char tJ = t[j - 1]; // jth character of t
                d[0] = j;

                for (i = 1; i <= n; i++)
                {
                    int cost = s[i - 1] == tJ ? 0 : 1; // cost
                                                       // minimum of cell to the left+1, to the top+1, diagonally left and up +cost                
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                // copy current distance counts to 'previous row' distance counts
                int[] dPlaceholder = p; //placeholder to assist in swapping p and d
                p = d;
                d = dPlaceholder;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts
            return p[n];
        }

        public static List<string> MaybeThought(this List<string> list, string typed, int maxDiffrence)
        {
            List<string> toReturn = new List<string>();

            foreach (string s in list)
                if (GetDiffrence(s, typed) <= maxDiffrence)
                    toReturn.Add(s);

            return toReturn;
        }

        public static void AddAll<T>(this List<T> list, List<T> toAdd)
        {
            foreach (T t in toAdd)
                list.Add(t);
        }

        public static string Remove(this string s, char toRemove)
        {
            string toReturn = "";

            foreach (char ch in s.ToCharArray())
                if (ch != toRemove)
                    toReturn += ch;

            return toReturn;
        }

        public static string[] GetLines(this string s) => s.Split('\n');

        public static bool IsNullOrEmptyOrWhiteSpace(this string s) => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);

        public static byte ToByte(this string s) => byte.Parse(s);
        public static short ToShort(this string s) => short.Parse(s);
        public static int ToInt(this string s) => int.Parse(s);
        public static float ToFloat(this string s) => float.Parse(s.Replace('.', ','));
        public static double ToDouble(this string s) => double.Parse(s.Replace('.', ','));


        /// <summary>
        ///  Returns: string in bigger for ex." A - 
        ///  ┌─┐
        ///  ├─┤
        ///  │ │.
        ///  When char not supported inserts ---.
        ///  Use Console.OutputEncoding = System.Text.Encoding.Unicode;!!!
        /// </summary>
        public static string ToBIG(this string s, bool thick = false, int _spaceBetweenLetters = 1)
        {
            if (Console.OutputEncoding != System.Text.Encoding.Unicode)
                Console.OutputEncoding = System.Text.Encoding.Unicode;

            string line1 = "";
            string line2 = "";
            string line3 = "";

            string space = new string(' ', _spaceBetweenLetters);/*"";
            for (int i = 0; i < _spaceBetweenLetters; i++)
                space += " ";*/

            Type t = typeof(GeneralExtensions);
            List<FieldInfo> fields = t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();

            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i].ToString().ToUpper()[0];
                if (ch == ' ')
                {
                    line1 += "   " + space;
                    line2 += "   " + space;
                    line3 += "   " + space;
                    continue;
                }
                else if (ch == '\n')
                {
                    line1 += "\n\n\n" + space;
                    line2 += "" + space;
                    line3 += "" + space;
                    continue;
                }
                else if (ch == '!')
                {
                    line1 += " │ " + space;
                    line2 += " │ " + space;
                    line3 += " ▪ " + space;
                    continue;
                }
                else if (ch == '?')
                {
                    line1 += "╶─┐" + space;
                    line2 += " ┌┘" + space;
                    line3 += " ▪ " + space;
                    continue;
                }
                else if (ch == ',')
                {
                    line1 += "   " + space;
                    line2 += "   " + space;
                    line3 += " ╷ " + space;
                    continue;
                }
                else if (ch == '.')
                {
                    line1 += "   " + space;
                    line2 += "   " + space;
                    line3 += " ▪ " + space;
                    continue;
                }
                else if (ch == ':')
                {
                    line1 += "   " + space;
                    line2 += " ▪ " + space;
                    line3 += " ▪ " + space;
                    continue;
                }
                else if (ch == '\\')
                {
                    line1 += "\\  " + space;
                    line2 += " \\ " + space;
                    line3 += "  \\" + space;
                    continue;
                }
                else if (ch == '/')
                {
                    line1 += "  /" + space;
                    line2 += " / " + space;
                    line3 += "/  " + space;
                    continue;
                }
                else if (ch == '&')
                {
                    line1 += "┌─┐" + space;
                    line2 += "├─┤" + space;
                    line3 += @"└─\" + space;
                    continue;
                }
                try
                {
                    FieldInfo info = fields.Find(f => f.Name == $"BIG_{ch}");
                    if (info == default)
                    {
                        line1 += "---" + space;
                        line2 += "---" + space;
                        line3 += "---" + space;
                        continue;
                    }
                    string BIGChar = (string)info.GetValue(null);
                    line1 += BIGChar.GetLines()[0] + space;
                    line2 += BIGChar.GetLines()[1] + space;
                    line3 += BIGChar.GetLines()[2] + space;
                }
                catch
                {
                    line1 += "---" + space;
                    line2 += "---" + space;
                    line3 += "---" + space;
                }
            }

            if (thick)
            {
                line1 = BIGToTHICK(line1);
                line2 = BIGToTHICK(line2);
                line3 = BIGToTHICK(line3);
            }

            return line1 + "\n" + line2 + "\n" + line3;
        }

        public static string BIGToTHICK(this string s)
        {
            string toReturn = "";
            for (int i = 0; i < s.Length; i++)
                switch (s[i])
                {
                    case '─':
                        toReturn += "━";
                        break;
                    case '│':
                        toReturn += "┃";
                        break;
                    case '┌':
                        toReturn += "┏";
                        break;
                    case '┐':
                        toReturn += "┓";
                        break;
                    case '└':
                        toReturn += "┗";
                        break;
                    case '┘':
                        toReturn += "┛";
                        break;
                    case '├':
                        toReturn += "┣";
                        break;
                    case '┤':
                        toReturn += "┫";
                        break;
                    case '┬':
                        toReturn += "┳";
                        break;
                    case '┴':
                        toReturn += "┻";
                        break;
                    case '┼':
                        toReturn += "╋";
                        break; //╵╹╷╻╴╸╶╺
                    case '╵':
                        toReturn += "╹";
                        break;
                    case '╷':
                        toReturn += "╻";
                        break;
                    case '╴':
                        toReturn += "╸";
                        break;
                    case '╶':
                        toReturn += "╺";
                        break;
                    case '▪':
                        toReturn += '■';
                        break;
                    default:
                        toReturn += s[i];
                        break;
                }

            return toReturn;
        }

        public const string BIG_A = "┌─┐\n├─┤\n╵ ╵"; //─│┌┐└┘├┤┬┴┼   ━┃┏┓┗┛┣┠┫┨┰┳┸┻╂╋   ╱╲
        public const string BIG_B = "├─┐\n├─┤\n├─┘"; //│ ╱
        public const string BIG_C = "┌─╴\n│  \n└─╴"; //├┤
        public const string BIG_D = "├─┐\n│ │\n├─┘"; //│ ╲
        public const string BIG_E = "┌─╴\n├─╴\n└─╴";
        public const string BIG_F = "┌─╴\n├─╴\n╵  ";
        public const string BIG_G = "┌─╴\n│ ┐\n└─┘";
        public const string BIG_H = "╷ ╷\n├─┤\n╵ ╵";
        public const string BIG_I = " ┬ \n │ \n ┴ ";
        public const string BIG_J = "  ╷\n  │\n└─┘";
        public const string BIG_K = "│ ╱\n├┤ \n│ ╲";
        public const string BIG_L = "╷  \n│  \n└─╴";
        public const string BIG_M = "┌┬┐\n│││\n╵╵╵";
        public const string BIG_N = "┌┐╷\n│││\n╵└┘";
        public const string BIG_O = "┌─┐\n│ │\n└─┘";
        public const string BIG_P = "┌─┐\n├─┘\n╵  ";
        public const string BIG_Q = "┌─┐\n│ │\n└─╲";
        public const string BIG_R = "┌─┐\n├┬┘\n╵ ╲";
        public const string BIG_S = "┌─╴\n└─┐\n╶─┘";
        public const string BIG_T = "─┬─\n │ \n ╵ ";
        public const string BIG_U = "╷ ╷\n│ │\n└─┘";
        public const string BIG_V = "╷ ╷\n│ │\n\\_/";
        public const string BIG_W = "╷╷╷\n│││\n└┴┘";
        public const string BIG_X = "\\ /\n ╳ \n/ \\";
        public const string BIG_Y = "╷ ╷\n\\_/\n │ ";
        public const string BIG_Z = "╶─┐\n ╱ \n└─╴";

        public const string BIG_0 = "┌┐\n││\n└┘";
        public const string BIG_1 = " /│\n  │\n  │";
        public const string BIG_2 = "┌─┐\n ╱ \n/__";
        public const string BIG_3 = "╶─┐\n╶─┤\n╶─┘";
        public const string BIG_4 = "╷ ╷\n└─┤\n  │";
        public const string BIG_5 = "┌─╴\n└─\\\n╶─/";
        public const string BIG_6 = "┌─╴\n├─┐\n└─┘";
        public const string BIG_7 = "╶─┐\n ╱ \n/  ";
        public const string BIG_8 = "┌─┐\n├─┤\n└─┘";
        public const string BIG_9 = "┌─┐\n└─┤\n  │";

        internal static Arrows GetArrow()
        {
            ConsoleKeyInfo info = Console.ReadKey(true);
            switch (info.Key)
            {
                case ConsoleKey.UpArrow:
                    return Arrows.Up;
                case ConsoleKey.DownArrow:
                    return Arrows.Down;
                case ConsoleKey.LeftArrow:
                    return Arrows.Left;
                case ConsoleKey.RightArrow:
                    return Arrows.Rigth;
                case ConsoleKey.Enter:
                    return Arrows.Enter;
                case ConsoleKey.Escape:
                    return Arrows.Escape;
                default:
                    return Arrows.None;
            }
        }

        public enum Arrows
        {
            Up,
            Down,
            Left,
            Rigth,
            Enter,
            Escape,
            None,
        }
    }
}
