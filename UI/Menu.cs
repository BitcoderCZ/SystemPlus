using System;
using SystemPlus.Vectors;
using static SystemPlus.Extensions.GeneralExtensions;

namespace SystemPlus.UI
{
    public sealed class Menu : IMenu<int>
    {
        private int selected;
        private string[] options;
        private string title;

        public Menu(string _title, string[] _options)
        {
            selected = 0;
            title = _title;
            options = _options;
        }

        public int Show(Vector2Int pos)
        {
            while (true)
            {
                Console.SetCursorPosition(pos.x, pos.y);
                Console.ResetColor();

                for (int i = 0; i < title.GetLines().Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    Console.Write(title.GetLines()[i] + "\n");
                }

                Console.Write("\n");

                for (int i = 0; i < options.Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine(options[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(options[i]);
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter)
                    return selected;

                selected = MathPlus.Clamp(selected, 0, options.Length);
            }
        }

        public int Show(Vector2Int pos, Image image)
        {
            while (true)
            {
                Console.SetCursorPosition(pos.x, pos.y);
                image.Print();
                Console.ResetColor();

                for (int i = 0; i < title.GetLines().Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    Console.Write(title.GetLines()[i] + "\n");
                }

                Console.Write("\n");

                for (int i = 0; i < options.Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine(options[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(options[i]);
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter)
                    return selected;

                selected = MathPlus.Clamp(selected, 0, options.Length);
            }
        }

        public int Show(Vector2Int pos, ConsoleImage image)
        {
            while (true)
            {
                Console.SetCursorPosition(pos.x, pos.y);
                image.Print();
                Console.ResetColor();

                for (int i = 0; i < title.GetLines().Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    Console.Write(title.GetLines()[i] + "\n");
                }

                Console.Write("\n");

                for (int i = 0; i < options.Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine(options[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(options[i]);
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter)
                    return selected;

                selected = MathPlus.Clamp(selected, 0, options.Length);
            }
        }

        public void Render(Vector2Int pos)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ResetColor();

            for (int i = 0; i < title.GetLines().Length; i++)
            {
                Console.CursorLeft = pos.x;
                Console.Write(title.GetLines()[i] + "\n");
            }

            Console.Write("\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = pos.x;
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(options[i]);
                }
            }

            Console.ResetColor();
        }

        public void Render(Vector2Int pos, Image image)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            image.Print();
            Console.ResetColor();

            for (int i = 0; i < title.GetLines().Length; i++)
            {
                Console.CursorLeft = pos.x;
                Console.Write(title.GetLines()[i] + "\n");
            }

            Console.Write("\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = pos.x;
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(options[i]);
                }
            }

            Console.ResetColor();
        }

        public void Render(Vector2Int pos, ConsoleImage image)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            image.Print();
            Console.ResetColor();

            for (int i = 0; i < title.GetLines().Length; i++)
            {
                Console.CursorLeft = pos.x;
                Console.Write(title.GetLines()[i] + "\n");
            }

            Console.Write("\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = pos.x;
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(options[i]);
                }
            }

            Console.ResetColor();
        }

        public void SetSelected(int _selected) => selected = MathPlus.Clamp(_selected, 0, options.Length);
    }
}
