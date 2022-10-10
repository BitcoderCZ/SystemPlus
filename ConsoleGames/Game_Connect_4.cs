using System;

namespace SystemPlus.ConsoleGames
{
    public sealed class Game_Connect_4
    {
        private bool updateDisp;
        private ColoredString display;
        public ColoredString Display
        {
            get
            {
                if (updateDisp)
                { //┏┓┗┛┣┫┳┻━┃╋
                    display = $"# 0015 ┏━━━━━━━┳━━━━━━━┳━━━━━━━┳━━━━━━━┳━━━━━━━┳━━━━━━━┳━━━━━━━┓\n" +
                              $" {GC(0, 0)}┃      # 0015 {GC(1, 0)}┃      # 0015 {GC(2, 0)}┃      # 0015 {GC(3, 0)}┃      # 0015 {GC(4, 0)}┃      # 0015 {GC(5, 0)}┃      # 0015 {GC(6, 0)}┃      # 0015 ┃\n" +
                              $" {GC(0, 0)}┃      # 0015 {GC(1, 0)}┃      # 0015 {GC(2, 0)}┃      # 0015 {GC(3, 0)}┃      # 0015 {GC(4, 0)}┃      # 0015 {GC(5, 0)}┃      # 0015 {GC(6, 0)}┃      # 0015 ┃\n" +
                              $" {GC(0, 0)}┃      # 0015 {GC(1, 0)}┃      # 0015 {GC(2, 0)}┃      # 0015 {GC(3, 0)}┃      # 0015 {GC(4, 0)}┃      # 0015 {GC(5, 0)}┃      # 0015 {GC(6, 0)}┃      # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━#┫0000                 # 0015 ┏━━━━━━━┓\n" +
                              $" {GC(0, 1)}┃      # 0015 {GC(1, 1)}┃      # 0015 {GC(2, 1)}┃      # 0015 {GC(3, 1)}┃      # 0015 {GC(4, 1)}┃      # 0015 {GC(5, 1)}┃      # 0015 {GC(6, 1)}┃      # 0015 #┃0000                 # 0015┃{PL()}       # 0015 ┃\n" +
                              $" {GC(0, 1)}┃      # 0015 {GC(1, 1)}┃      # 0015 {GC(2, 1)}┃      # 0015 {GC(3, 1)}┃      # 0015 {GC(4, 1)}┃      # 0015 {GC(5, 1)}┃      # 0015 {GC(6, 1)}┃      # 0015 #┃0000                 # 0015┃{PL()}       # 0015 ┃\n" +
                              $" {GC(0, 1)}┃      # 0015 {GC(1, 1)}┃      # 0015 {GC(2, 1)}┃      # 0015 {GC(3, 1)}┃      # 0015 {GC(4, 1)}┃      # 0015 {GC(5, 1)}┃      # 0015 {GC(6, 1)}┃      # 0015 #┃0000                 # 0015┃{PL()}       # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━#┫0000                 # 0015 ┗━━━━━━━┛\n" +
                              $" {GC(0, 2)}┃      # 0015 {GC(1, 2)}┃      # 0015 {GC(2, 2)}┃      # 0015 {GC(3, 2)}┃      # 0015 {GC(4, 2)}┃      # 0015 {GC(5, 2)}┃      # 0015 {GC(6, 2)}┃      # 0015 ┃\n" +
                              $" {GC(0, 2)}┃      # 0015 {GC(1, 2)}┃      # 0015 {GC(2, 2)}┃      # 0015 {GC(3, 2)}┃      # 0015 {GC(4, 2)}┃      # 0015 {GC(5, 2)}┃      # 0015 {GC(6, 2)}┃      # 0015 ┃\n" +
                              $" {GC(0, 2)}┃      # 0015 {GC(1, 2)}┃      # 0015 {GC(2, 2)}┃      # 0015 {GC(3, 2)}┃      # 0015 {GC(4, 2)}┃      # 0015 {GC(5, 2)}┃      # 0015 {GC(6, 2)}┃      # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━#┫0000        # 0015 ┏━━━━━━━#┓0000        # 0015 ┏━━━━━━━┓\n" +
                              $" {GC(0, 3)}┃      # 0015 {GC(1, 3)}┃      # 0015 {GC(2, 3)}┃      # 0015 {GC(3, 3)}┃      # 0015 {GC(4, 3)}┃      # 0015 {GC(5, 3)}┃      # 0015 {GC(6, 3)}┃      # 0015 #┃0000        # 0015┃#┃0009    /┃ # 0015 #┃0000        # 0015┃#┃0012   ┏━┓ # 0015 ┃\n" +
                              $" {GC(0, 3)}┃      # 0015 {GC(1, 3)}┃      # 0015 {GC(2, 3)}┃      # 0015 {GC(3, 3)}┃      # 0015 {GC(4, 3)}┃      # 0015 {GC(5, 3)}┃      # 0015 {GC(6, 3)}┃      # 0015 #┃0000        # 0015┃#┃0009     ┃ # 0015 #┃0000        # 0015┃#┃0012    ╱  # 0015 ┃\n" +
                              $" {GC(0, 3)}┃      # 0015 {GC(1, 3)}┃      # 0015 {GC(2, 3)}┃      # 0015 {GC(3, 3)}┃      # 0015 {GC(4, 3)}┃      # 0015 {GC(5, 3)}┃      # 0015 {GC(6, 3)}┃      # 0015 #┃0000        # 0015┃#┃0009     ┃ # 0015 #┃0000        # 0015┃#┃0012   ┗━━ # 0015 ┃\n" +
                              $"# 0015 ┣━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━╋━━━━━━━#┫0000        # 0015 ┗━━━━━━━#┛0000        # 0015 ┗━━━━━━━┛\n" +
                              $" {GC(0, 4)}┃      # 0015 {GC(1, 4)}┃      # 0015 {GC(2, 4)}┃      # 0015 {GC(3, 4)}┃      # 0015 {GC(4, 4)}┃      # 0015 {GC(5, 4)}┃      # 0015 {GC(6, 4)}┃      # 0015 ┃\n" +
                              $" {GC(0, 4)}┃      # 0015 {GC(1, 4)}┃      # 0015 {GC(2, 4)}┃      # 0015 {GC(3, 4)}┃      # 0015 {GC(4, 4)}┃      # 0015 {GC(5, 4)}┃      # 0015 {GC(6, 4)}┃      # 0015 ┃\n" +
                              $" {GC(0, 4)}┃      # 0015 {GC(1, 4)}┃      # 0015 {GC(2, 4)}┃      # 0015 {GC(3, 4)}┃      # 0015 {GC(4, 4)}┃      # 0015 {GC(5, 4)}┃      # 0015 {GC(6, 4)}┃      # 0015 ┃\n" +
                              $"# 0015 ┗━━━━━━━┻━━━━━━━┻━━━━━━━┻━━━━━━━┻━━━━━━━┻━━━━━━━┻━━━━━━━┛\n" +
                              $"# 0015     0       1       2       3       4       5       6   # 0000{new string(' ', 20)}\n";
                    updateDisp = false;
                }
                return display;
            }
        }
        private int[] grid;
        private int[] winPoses;

        public int win { get; private set; }

        private string GC(int x, int y)
        {
            if (win != -1 && IsWinPos(y * width + x))
            {
                if (win == 1)
                    return "#┃0010"; // Green
                else
                    return "#┃0013"; // Purple
            }
            int value = grid[y * width + x];
            switch (value)
            {
                case -1:
                    return "#┃0015"; // white
                case 1:
                    return "#┃0009"; // blue
                case 2:
                    return "#┃0012"; // red
                default:
                    return "#┃0015"; // white
            }
        }
        private string PL() => player1 ? "#┃0009" : "#┃0012";

        const int width = 7;
        const int height = 5;

        bool player1;

        public Game_Connect_4()
        {
            grid = new int[width * height];
            winPoses = new int[4];

            Reset();
        }

        public void Set(int xPos)
        {
            if (xPos < 0 || xPos >= width)
                return;

            if (grid[xPos] != -1)
                return;

            if (win != -1)
                return;

            updateDisp = true;

            for (int i = height - 1; i >= 0; i--)
                if (grid[i * width + xPos] == -1)
                {
                    grid[i * width + xPos] = player1 ? 1 : 2;
                    player1 = !player1;
                    break;
                }

            CheckWin();
        }

        public void Reset()
        {
            for (int i = 0; i < grid.Length; i++)
                grid[i] = -1;

            for (int i = 0; i < winPoses.Length; i++)
                winPoses[i] = -1;

            player1 = true;
            win = -1;
            updateDisp = true;
        }

        private void CheckWin()
        {
            // xxxx
            for (int i = 0; i < grid.Length - 3; i++)
            {
                if (i % width == 4)
                {
                    i += 2;
                    continue;
                }
                else if (grid[i] == -1)
                    continue;

                int val = grid[i];
                bool no = false;
                for (int j = 1; j < 4; j++)
                    if (grid[i + j] != val)
                    {
                        no = true;
                        break;
                    }
                if (no)
                    continue;

                for (int j = 0; j < 4; j++)
                    winPoses[j] = i + j;

                win = val;

                updateDisp = true;
                return;
            }

            //x
            //x
            //x
            //x
            for (int i = 0; i < width * 2; i++)
            {
                if (grid[i] == -1)
                    continue;

                int val = grid[i];
                bool no = false;
                for (int j = 1; j < 4; j++)
                    if (grid[i + j * width] != val)
                    {
                        no = true;
                        break;
                    }
                if (no)
                    continue;

                for (int j = 0; j < 4; j++)
                    winPoses[j] = i + j * width;

                win = val;

                updateDisp = true;
                return;
            }
            //   x
            //  x
            // x
            //x
            for (int i = 3; i < width * 2; i++)
            {
                if (i % width == 0)
                {
                    i += 2;
                    continue;
                }

                if (grid[i] == -1)
                    continue;

                int val = grid[i];
                bool no = false;
                for (int j = 1; j < 4; j++)
                    if (grid[i + j * width - j] != val)
                    {
                        no = true;
                        break;
                    }
                if (no)
                    continue;

                for (int j = 0; j < 4; j++)
                    winPoses[j] = i + j * width - j;

                win = val;

                updateDisp = true;
                return;
            }

            //x
            // x
            //  x
            //   x
            for (int i = 0; i < width * 2 - 3; i++)
            {
                if (i % width == width - 2)
                {
                    i++;
                    continue;
                }

                if (grid[i] == -1)
                    continue;

                int val = grid[i];
                bool no = false;
                for (int j = 1; j < 4; j++)
                    if (grid[i + j * width + j] != val)
                    {
                        no = true;
                        break;
                    }
                if (no)
                    continue;

                for (int j = 0; j < 4; j++)
                    winPoses[j] = i + j * width + j;

                win = val;

                updateDisp = true;
                return;
            }
        }

        private bool IsWinPos(int index)
        {
            for (int i = 0; i < 4; i++)
                if (winPoses[i] == index)
                    return true;

            return false;
        }
    }
}
