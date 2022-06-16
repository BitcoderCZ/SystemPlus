using System;

namespace SystemPlus.ConsoleGames
{
    public sealed class Game_2048
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
                              $" {GC(0, 2)}┃ {pos(0, 2)}# 0015 {GC(1, 2)}┃ {pos(1, 2)}# 0015 {GC(2, 2)}┃ {pos(2, 2)}# 0015 {GC(3, 2)}┃ {pos(3, 2)}# 0015 ┃\n" +
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
        private int[] grid;

        private string GC(int x, int y)
        {
            int value = grid[y * 4 + x];
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
            if (grid[y * 4 + x] == -1)
                return "     ";
            int spaces = 5 - grid[y * 4 + x].ToString().Length;
            string spacesS = "";
            for (int i = 0; i < spaces; i++)
                spacesS += " ";
            return grid[y * 4 + x] + spacesS;
        }

        public Game_2048()
        {
            random = new Random();

            grid = new int[16];
            for (int i = 0; i < 16; i++)
                grid[i] = -1;

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
            ArrayRef<int> current;
            ArrayRef<int> up;
            for (int r = 0; r < 4; r++) // make sure peaces move all the way, not just by one space
                for (int i = 4; i < 16; i++)
                {
                    current = new ArrayRef<int>(i, grid);
                    up = new ArrayRef<int>(i - 4, grid);

                    if (current != -1)
                        if (up == -1)
                        {
                            up.Value = current;
                            current.Value = -1;
                        }
                        else if (up.Value == current)
                        {
                            up.Value = current * 2;
                            current.Value = -1;
                        }
                }
        }
        private void MoveDown()
        {
            ArrayRef<int> current;
            ArrayRef<int> down;
            for (int r = 0; r < 4; r++) // make sure peaces move all the way, not just by one space
                for (int i = 11; i >= 0; i--)
                {
                    current = new ArrayRef<int>(i, grid);
                    down = new ArrayRef<int>(i + 4, grid);

                    if (current != -1)
                        if (down == -1)
                        {
                            down.Value = current;
                            current.Value = -1;
                        }
                        else if (down.Value == current)
                        {
                            down.Value = current * 2;
                            current.Value = -1;
                        }
                }
        }
        private void MoveLeft()
        {
            ArrayRef<int> current;
            ArrayRef<int> left;
            for (int r = 0; r < 4; r++) // make sure peaces move all the way, not just by one space
                for (int i = 1; i < 16; i++)
                {
                    if (i % 4 == 0)
                        continue;

                    current = new ArrayRef<int>(i, grid);
                    left = new ArrayRef<int>(i - 1, grid);

                    if (current != -1)
                        if (left == -1)
                        {
                            left.Value = current;
                            current.Value = -1;
                        }
                        else if (left.Value == current)
                        {
                            left.Value = current * 2;
                            current.Value = -1;
                        }
                }
        }
        private void MoveRight()
        {
            ArrayRef<int> current;
            ArrayRef<int> right;
            for (int r = 0; r < 4; r++) // make sure peaces move all the way, not just by one space
                for (int i = 14; i >= 0; i--)
                {
                    if (i % 4 == 3)
                        continue;

                    current = new ArrayRef<int>(i, grid);
                    right = new ArrayRef<int>(i + 1, grid);

                    if (current != -1)
                        if (right == -1)
                        {
                            right.Value = current;
                            current.Value = -1;
                        }
                        else if (right.Value == current)
                        {
                            right.Value = current * 2;
                            current.Value = -1;
                        }
                }
        }

        private bool SetToRandom(int v)
        {
            for (int i = 0; i < 16; i++)
                if (grid[i] == -1)
                    goto set;

            return true;

        set:
            while (true)
            {
                int i = random.Next(0, 16);
                if (grid[i] == -1)
                {
                    grid[i] = v;
                    return false;
                }
            }
        }
    }
}
