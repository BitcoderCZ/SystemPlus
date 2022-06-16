using System;

namespace SystemPlus.Obsolete
{
    [Obsolete("Does not work!")]
    public class _Game_2048
    {
        private bool updateDisp;
        private string display;
        public string Display => new ColoredString(DisplayColored).ToString();
        public string DisplayColored
        {
            get
            {
                if (updateDisp)
                { //┏┓┗┛┣┫┳┻━┃╋
                    display = $"# 0015 ┏━━━━━━━┳━━━━━━━┳━━━━━━━┳━━━━━━━┓\n" +
                              $" {GC(0, 0)}┃      # 0015 {GC(1, 0)}┃      # 0015 {GC(2, 0)}┃      # 0015 {GC(3, 0)}┃      # 0015 ┃\n" +
                              $" {GC(0, 0)}┃ {pos(0, 0)}# 0015 {GC(1, 0)}┃ {pos(1, 0)}# 0015 {GC(2, 0)}┃ {pos(2, 0)}# 0015 {GC(3, 0)}┃ {pos(3, 0)}# 0015 ┃\n" +
                              $" {GC(0, 0)}┃      # 0015 {GC(1, 0)}┃      # 0015 {GC(2, 0)}┃      # 0015 {GC(3, 0)}┃      # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━┫\n" +
                              $" {GC(0, 1)}┃      # 0015 {GC(1, 1)}┃      # 0015 {GC(2, 1)}┃      # 0015 {GC(3, 1)}┃      # 0015 ┃\n" +
                              $" {GC(0, 1)}┃ {pos(0, 1)}# 0015 {GC(1, 1)}┃ {pos(1, 1)}# 0015 {GC(2, 1)}┃ {pos(2, 1)}# 0015 {GC(3, 1)}┃ {pos(3, 1)}# 0015 ┃\n" +
                              $" {GC(0, 1)}┃      # 0015 {GC(1, 1)}┃      # 0015 {GC(2, 1)}┃      # 0015 {GC(3, 1)}┃      # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━┫\n" +
                              $" {GC(0, 2)}┃      # 0015 {GC(1, 2)}┃      # 0015 {GC(2, 2)}┃      # 0015 {GC(3, 2)}┃      # 0015 ┃\n" +
                              $" {GC(0, 2)}┃ {pos(0, 2)}# 0015 {GC(1, 2)}┃ {pos(1, 2)}# 0015 {GC(2, 2)}┃ {pos(2, 2)}# 0015 {GC(3, 2)}┃ {pos(3, 3)}# 0015 ┃\n" +
                              $" {GC(0, 2)}┃      # 0015 {GC(1, 2)}┃      # 0015 {GC(2, 2)}┃      # 0015 {GC(3, 2)}┃      # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━┫\n" +
                              $" {GC(0, 3)}┃      # 0015 {GC(1, 3)}┃      # 0015 {GC(2, 3)}┃      # 0015 {GC(3, 3)}┃      # 0015 ┃\n" +
                              $" {GC(0, 3)}┃ {pos(0, 3)}# 0015 {GC(1, 3)}┃ {pos(1, 3)}# 0015 {GC(2, 3)}┃ {pos(2, 3)}# 0015 {GC(3, 3)}┃ {pos(3, 3)}# 0015 ┃\n" +
                              $" {GC(0, 3)}┃      # 0015 {GC(1, 3)}┃      # 0015 {GC(2, 3)}┃      # 0015 {GC(3, 3)}┃      # 0015 ┃\n" +
                              $"# 0015 ┗━━━━━━━┻━━━━━━━┻━━━━━━━┻━━━━━━━┛\n";
                    updateDisp = false;
                }
                return display;
            }
        }
        private int[,] grid;

        private string GC(int x, int y)
        {
            int value = grid[x, y];
            switch (value)
            {
                case -1:
                    return "#┃0015";//"# 1515";
                case 1:
                    return "#┃0004";
                case 2:
                    return "#┃0012";
                case 4:
                    return "#┃0006";
                case 8:
                    return "#┃0014";
                case 16:
                    return "#┃0010";
                case 32:
                    return "#┃0002";
                case 64:
                    return "#┃0011";
                case 128:
                    return "#┃0009";
                case 256:
                    return "#┃0003";
                case 512:
                    return "#┃0001";
                case 1024:
                    return "#┃0005";
                case 2048:
                    return "#┃0013";
                default:
                    return "#┃0013";
            }
        }

        private Random random;

        private string pos(int x, int y)
        {
            if (grid[x, y] == -1)
                return "     ";
            int spaces = 5 - grid[x, y].ToString().Length;
            string spacesS = "";
            for (int i = 0; i < spaces; i++)
                spacesS += " ";
            return grid[x, y] + spacesS;
        }

        public _Game_2048()
        {
            random = new Random();

            grid = new int[4, 4];
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    grid[x, y] = -1;

            SetToRandom(1);

            updateDisp = true;
        }

        /// <summary>
        /// Returns true if game is lost or if input is error.
        /// </summary>
        public bool SetInput(Input input)
        {
            switch (input)
            {
                case Input.Up:
                    MoveUp();
                    break;
                case Input.Down:
                    MoveDown();
                    break;
                case Input.Left:
                    MoveLeft();
                    break;
                case Input.Right:
                    MoveRight();
                    break;
                case Input.ERROR:
                    return true;
            }

            updateDisp = true;

            return SetToRandom(1);
        }

        public enum Input
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
            ERROR = 4,
        }

        public Input ToInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    return Input.Left;
                case ConsoleKey.UpArrow:
                    return Input.Up;
                case ConsoleKey.RightArrow:
                    return Input.Right;
                case ConsoleKey.DownArrow:
                    return Input.Down;
                default:
                    return Input.ERROR;
            }
        }

        private void MoveUp()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int x = 0; x < 4; x++)
                    for (int y = 1; y < 4; y++)
                    {
                        for (int i = y; i >= 1; i--)
                            if (grid[x, y] != -1)
                            {
                                if (grid[x, y - 1] == -1)
                                {
                                    grid[x, y - 1] = grid[x, y];
                                    grid[x, y] = -1;
                                }
                                else if (grid[x, y - 1] == grid[x, y])
                                {
                                    grid[x, y - 1] += grid[x, y - 1];
                                    grid[x, y] = -1;
                                }
                            }
                    }
            }
        }
        private void MoveDown()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int x = 0; x < 4; x++)
                    for (int y = 2; y >= 0; y--)
                    {
                        for (int i = 0; i <= y; i++)
                            if (grid[x, y] != -1)
                            {
                                if (grid[x, y + 1] == -1)
                                {
                                    grid[x, y + 1] = grid[x, y];
                                    grid[x, y] = -1;
                                }
                                else if (grid[x, y + 1] == grid[x, y])
                                {
                                    grid[x, y + 1] += grid[x, y + 1];
                                    grid[x, y] = -1;
                                }
                            }
                    }
            }
        }
        private void MoveLeft()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int y = 0; y < 4; y++)
                    for (int x = 1; x < 4; x++)
                    {
                        for (int i = x; i >= 1; i--)
                            if (grid[x, y] != -1)
                            {
                                if (grid[x - 1, y] == -1)
                                {
                                    grid[x - 1, y] = grid[x, y];
                                    grid[x, y] = -1;
                                }
                                else if (grid[x, y - 1] == grid[x, y])
                                {
                                    grid[x - 1, y] += grid[x - 1, y];
                                    grid[x, y] = -1;
                                }
                            }
                    }
            }
        }
        private void MoveRight()
        {

        }

        private bool SetToRandom(int i)
        {
            for (int f454dgdr = 0; f454dgdr < 10000; f454dgdr++)
            {
                int x = random.Next(0, 5);
                int y = random.Next(0, 5);
                if (grid[x, y] != -1)
                    continue;
                else
                {
                    grid[x, y] = i;
                    return false;
                }
            }

            return true;
        }
    }
}
