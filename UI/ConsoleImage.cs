using System;
using System.Drawing;
using System.Threading.Tasks;
using SystemPlus.Extensions;
using SystemPlus.Utils;

namespace SystemPlus.UI
{
    public class ConsoleImage : Buffer2D<byte>
    {
        public new short Width { get => (short)base.Width; }
        public new short Height { get => (short)base.Height; }

        public ConsoleImage(Size size)
            : base(new Size((short)size.Width, (short)size.Height))
        {
            Data.Fill((byte)ConsoleColor.Black);
        }

        public ConsoleImage(Size size, ConsoleColor[] data)
            : base(new Size((short)size.Width, (short)size.Height), data.ToNewType((ConsoleColor cc) => (byte)cc))
        { }

        public void Print()
        {
            int XStart = Console.CursorLeft;

            ConsoleColor lastColor = (ConsoleColor)Data[0];
            Console.BackgroundColor = (ConsoleColor)Data[0];
            string toPrint = "";

            for (int i = 0; i < Data.Length; i++)
            {
                if (i % Width == 0 && i != 0 && toPrint != "")
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = (ConsoleColor)Data[i];
                    Console.SetCursorPosition(XStart, Console.CursorTop + 1);
                    continue;
                }

                if ((ConsoleColor)Data[i] == lastColor)
                {
                    toPrint += "  ";
                }
                else
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = (ConsoleColor)Data[i];
                }
            }

            if (toPrint != "")
            {
                Console.BackgroundColor = lastColor;
                Console.Write(toPrint);
            }

            Console.Write("\n");
            Console.ResetColor();
        }

        public void PrintFlip()
        {
            int XStart = Console.CursorLeft;

            ConsoleColor lastColor = (ConsoleColor)Data[0];
            Console.BackgroundColor = (ConsoleColor)Data[0];
            string toPrint = "";

            Console.CursorTop += Height;

            for (int i = 0; i < Data.Length; i++)
            {
                if (i % Width == 0 && i != 0 && toPrint != "")
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = (ConsoleColor)Data[i];
                    Console.SetCursorPosition(XStart, Console.CursorTop - 1);
                    continue;
                }

                if ((ConsoleColor)Data[i] == lastColor)
                {
                    toPrint += "  ";
                }
                else
                {
                    Console.BackgroundColor = lastColor;
                    Console.Write(toPrint);
                    toPrint = "  ";
                    lastColor = (ConsoleColor)Data[i];
                }
            }

            if (toPrint != "")
            {
                Console.BackgroundColor = lastColor;
                Console.Write(toPrint);
            }

            Console.CursorTop += Height + 1;
            Console.ResetColor();
        }

        public static ConsoleImage FromDirectBitmap(DirectBitmap db)
        {
            ConsoleColor[] data = new ConsoleColor[db.Width * db.Height];

            Parallel.For(0, data.Length, (int i) =>
            {
                data[i] = Color.FromARGB(db[i]).ToConsoleColor();
            });

            return new ConsoleImage(db.Size, data);
        }
    }
}
