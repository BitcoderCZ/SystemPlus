//#define G2048
//#define GC4

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemPlus.AI;
using SystemPlus.Extensions;
using SystemPlus.UI;
using SystemPlus.Utils;
using SystemPlus.Vectors;
using SystemPlus.Windows;
using WindowsInput;
using WindowsInput.Native;
using static SystemPlus.Extensions.GeneralExtensions;
using static SystemPlus.MouseOperations;
using Color = System.Drawing.Color;
using SFile = System.IO.File;

namespace SystemPlus.Test
{
    /*class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
#if G2048
            Game_2048 game = new Game_2048();
            while (true)
            {
                if (game.SetInput(game.ToInput(Console.ReadKey(true).Key)))
                    break;
                Console.SetCursorPosition(0, 0);
                new ColoredString(game.DisplayColored).Print();
                //Console.Write(game.Display);
            }
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("You Lost!".ToBIG());
            Thread.Sleep(2000);
            while (Console.KeyAvailable)
                Console.ReadKey(true);
                
            Console.ReadKey();
            Console.Clear();
            Main(null);
#elif GC4
            Game_Connect_4 game = new Game_Connect_4();
            Console.SetCursorPosition(0, 0);
            game.Display.Print();
            while (true)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                if (pressed.Key == ConsoleKey.R)
                {
                    game.Reset();
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    game.Display.Print();
                    continue;
                } else if (int.TryParse(pressed.KeyChar.ToString(), out int space))
                {
                    game.Set(space);
                    Console.SetCursorPosition(0, 0);
                    game.Display.Print();

                    if (game.win != -1)
                    {
                        Console.SetCursorPosition(0, 0);
                        game.Display.Print();
                        break;
                    }
                }
            }

            Thread.Sleep(500);
            Console.WriteLine($"\nPlayer {game.win} wins!".ToBIG());
            Thread.Sleep(1000);
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            Console.ReadKey();
            Console.Clear();
            Main(null);

#else

            string original = "Ahoj, jak se mas? Ja se mam dobre :)";
            string encoded = Encoding.stringEncoding.Encode(original, "abc123");
            string decoded = Encoding.stringEncoding.Decode(encoded, "abc123");
            string decodedWront = Encoding.stringEncoding.Decode(encoded, "abc124");
            Console.WriteLine("Original: " + original + "  " + original.Length);
            Console.WriteLine("Encoded: " + encoded + "  " + encoded.Length);
            Console.WriteLine("Decoded: " + decoded + "  " + decoded.Length);
            Console.WriteLine("Decoded wrong: " + decodedWront + "  " + decodedWront.Length);
            Console.ReadKey();

#endif

        }
    }*/
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        private static Dictionary<Key, KeyStates> keyStates = new Dictionary<Key, KeyStates>();

        private static Array keys = Enum.GetValues(typeof(Key));

        [STAThread]
        static void Main(string[] args)
        {
            //ConsoleExtensions.SetWindowPosition(2000, 20, 700, 400);
            ConsoleExtensions.Maximaze();

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            MenuSettings menu = new MenuSettings("Settings", new IMenuSettingsItem[]
            {
                new MSIIntSlider("int", 0, 0, 80) { SliderLength = 20},
            });

            menu.Show(Vector2Int.Zero);

            while (true) Console.ReadKey(true);

            Console.WriteLine(SaveUtil.CreateArray(false,
                SaveUtil.CreateObject(("name", "pepa"), ("height", "180")),
                SaveUtil.CreateObject(("name", "simon"), ("height", "170"))));

            while (true) Console.ReadKey(true);

            Console.Clear();

            ConsoleWindow console = ConsoleWindow.Create("Test");

            console.Write("Hello\nWorld");

            while (true)
            {
                Console.WriteLine(console.Read(false));
                System.Threading.Thread.Sleep(250);
            }

           /* Console.ReadKey();

            UI.Menu m = new UI.Menu("Select", new string[] { "Create image", "Train", "Load" });

            int sel = m.Show(new Vector2Int(4, 2));

            Console.Clear();

            int BLACK = Color.FromArgb(0, 0, 0).ToArgb();
            int WHITE = Color.FromArgb(255, 255, 255).ToArgb();

            if (sel == 0)
            {
                int imgNumb = 0;

                InputSimulator IS = new InputSimulator();

                for (int i = 0; i < keys.Length; i++)
                    keyStates[(Key)keys.GetValue(i)] = KeyStates.None;

                while (true)
                {
                    if (PressDown(Key.Escape))
                        Environment.Exit(0);
                    else if (PressDown(Key.Enter))
                    {
                        Console.Write("New image number: ");
                        imgNumb = int.Parse(ReadLine(true));
                        Console.Clear();
                        Console.Write("Selected: " + imgNumb);
                        Wait(1000);
                        Console.Clear();
                    }
                    else if (PressDown(Key.S))
                    {
                        if (Press(Key.LeftShift))
                        {
                            Save(imgNumb, IS);
                            Wait(500);
                            New(IS);
                            Wait(500);
                            Zoom();
                            Wait(500);
                        }
                        else
                            Save(imgNumb, IS);
                        imgNumb++;
                    }
                    else if (PressDown(Key.N))
                        New(IS);
                    else if (PressDown(Key.Y))
                        Zoom();
                    else if (PressDown(Key.Delete))
                        Delete();

                    UpdateKeys();
                }
            }
            else if (sel == 1)
            {
                Console.WriteLine("Loading images");

                string[] files = Directory.GetFiles("E:\\NeuralNetwork\\");

                float[][] images = new float[files.Length][];

                for (int i = 0; i < files.Length; i++)
                {
                    DirectBitmap db = DirectBitmap.LoadSafe(files[i]);
                    float[] img = new float[db.Data.Length];
                    Parallel.For(0, img.Length, (int j) =>
                    {
                        if (db.Data[j] == WHITE)
                            img[j] = 1;
                        else if (db.Data[j] == BLACK)
                            img[j] = 0;
                    });
                    images[i] = img;
                }


                Console.WriteLine("Training");

                NeuralNetwork n;

                Console.Write("Continue: ");
                if (Console.ReadKey().KeyChar == 'y')
                    n = NeuralNetwork.Load("E:\\NeuralNetwork.save");
                else
                    n = new NeuralNetwork(new int[] { 2500, 2000, 2000, 1000, 1000, 500, 500, 500, 200, 1 });

                Console.Write("\n");

                int bIndex = 6;

                Console.Write("Iterations: ");

                int iter = int.Parse(Console.ReadLine());

                for (int i = 0; i < iter; i++)
                {
                    for (int j = 0; j < images.Length; j++)
                    //Parallel.For(0, images.Length, (int j) =>
                    {
                        n.FeedForward(images[j]);
                        n.BackProp(new float[] { (j < bIndex) ? -1f : 1f });
                    }//);
                    Console.WriteLine("Done gen" + i);
                }

                Console.WriteLine("Results:\n");

                for (int j = 0; j < images.Length; j++)
                {
                    Console.WriteLine(n.FeedForward(images[j])[0]);
                }

                Console.Write("Save: ");
                if (Console.ReadKey().KeyChar == 'y')
                    NeuralNetwork.Save(n, "E:\\NeuralNetwork.save");
                Console.Write("\n");

                Console.ReadKey(true);

                while (true)
                {
                    Console.Clear();
                    MenuOpenFile ofd = new MenuOpenFile("Select file");

                    MenuOpenFile.STATUS dr = ofd.Show(new Vector2Int(4, 2));
                    Console.Clear();


                    if (dr != MenuOpenFile.STATUS.OK)
                        continue;

                    DirectBitmap db = DirectBitmap.LoadSafe(ofd.Selected);
                    float[] img = new float[db.Data.Length];
                    Parallel.For(0, img.Length, (int j) =>
                    {
                        if (db.Data[j] == WHITE)
                            img[j] = 1;
                        else if (db.Data[j] == BLACK)
                            img[j] = 0;
                    });

                    float res = n.FeedForward(img)[0];

                    Console.WriteLine((res < 0f ? "A" : "B") + "  val: " + res);
                    Console.ReadKey(true);
                }
            }
            else
            {
                Console.WriteLine("Loading images");

                string[] files = Directory.GetFiles("E:\\NeuralNetwork\\");

                float[][] images = new float[files.Length][];

                for (int i = 0; i < files.Length; i++)
                {
                    DirectBitmap db = DirectBitmap.LoadSafe(files[i]);
                    float[] img = new float[db.Data.Length];
                    Parallel.For(0, img.Length, (int j) =>
                    {
                        if (db.Data[j] == WHITE)
                            img[j] = 1;
                        else if (db.Data[j] == BLACK)
                            img[j] = 0;
                    });
                    images[i] = img;
                }

                NeuralNetwork n = NeuralNetwork.Load("E:\\NeuralNetwork.save");

                Console.WriteLine("Results:\n");

                for (int j = 0; j < images.Length; j++)
                {
                    Console.WriteLine(n.FeedForward(images[j])[0]);
                }

                Console.ReadKey(true);

                while (true)
                {
                    Console.Clear();
                    MenuOpenFile ofd = new MenuOpenFile("Select file");

                    MenuOpenFile.STATUS dr = ofd.Show(new Vector2Int(4, 2));
                    Console.Clear();


                    if (dr != MenuOpenFile.STATUS.OK)
                        continue;

                    DirectBitmap db = DirectBitmap.LoadSafe(ofd.Selected);
                    float[] img = new float[db.Data.Length];
                    Parallel.For(0, img.Length, (int j) =>
                    {
                        if (db.Data[j] == WHITE)
                            img[j] = 1;
                        else if (db.Data[j] == BLACK)
                            img[j] = 0;
                    });

                    float res = n.FeedForward(img)[0];

                    Console.WriteLine((res < 0f ? "A" : "B") + "  val: " + res);
                    Console.ReadKey(true);
                }
            }
           */
            #region nn
            /*NeuralNetwork net = new NeuralNetwork(new int[] { 2, 25, 25, 1 });

            float step = 1f / 3f; // numb - 1

            for (int i = 0; i < 10000; i++)
            {

                net.FeedForward(new float[] { 0, 0 });
                net.BackProp(new float[] { 0 });

                net.FeedForward(new float[] { 0, 1 });
                net.BackProp(new float[] { step });

                net.FeedForward(new float[] { 1, 0 });
                net.BackProp(new float[] { step + step });

                net.FeedForward(new float[] { 1, 1 });
                net.BackProp(new float[] { 1 });
            }

            Console.WriteLine($"{step}, {step + step}\n");
            Console.WriteLine(MathPlus.RoundToInt(net.FeedForward(new float[] { 0, 0 })[0] * 3));
            Console.WriteLine(MathPlus.RoundToInt(net.FeedForward(new float[] { 0, 1 })[0] * 3));
            Console.WriteLine(MathPlus.RoundToInt(net.FeedForward(new float[] { 1, 0 })[0] * 3));
            Console.WriteLine(MathPlus.RoundToInt(net.FeedForward(new float[] { 1, 1 })[0] * 3));*/
            #endregion
        }

        private static string ReadLine(bool disp)
        {
            string s = "";
            long count = 0;
            while (true)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Key k = (Key)keys.GetValue(i);
                    if (k == Key.None)
                        continue;
                    if (PressDown(k))
                    {
                        if (k == Key.Enter && count > 10)
                            return s;
                        else if (k == Key.Back && s.Length > 0)
                        {
                            s = s.Substring(0, s.Length - 1);
                            if (disp)
                            {
                                Console.CursorLeft--;
                                Console.Write(' ');
                                Console.CursorLeft--;
                            }
                        }
                        else
                        {
                            s += KeyToString(k);
                            if (disp)
                                Console.Write(KeyToString(k));
                        }
                    }
                }

                UpdateKeys();
                count++;
            }
        }

        private static string KeyToString(Key key)
        {
            string keyS = Enum.GetName(typeof(Key), key);
            if (keyS.Length == 1 && char.IsLetter(keyS[0]))
            {
                if (Press(Key.LeftShift))
                    return keyS;
                else
                    return char.ToLower(keyS[0]).ToString();
            }
            else if (keyS.Length == 7 && keyS.Substring(0, 6) == "NumPad")
                return keyS[6].ToString();
            else
                return "";
        }

        private static void UpdateKeys()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                Key k = (Key)keys.GetValue(i);
                if (k == Key.None)
                    continue;
                keyStates[k] = Keyboard.GetKeyStates(k);
            }
        }

        private static bool PressDown(Key key) => (Keyboard.GetKeyStates(key) & KeyStates.Down) == KeyStates.Down &&
                                                    (keyStates[key] == KeyStates.None || keyStates[key] == KeyStates.Toggled);
        private static bool Press(Key key) => (Keyboard.GetKeyStates(key) & KeyStates.Down) == KeyStates.Down;
        private static bool PressUp(Key key) => (Keyboard.GetKeyStates(key) == KeyStates.None || Keyboard.GetKeyStates(key) == KeyStates.Toggled) &&
                                                (keyStates[key] & KeyStates.Down) == KeyStates.Down;

        private static void Delete()
        {
            UI.Menu menu = new UI.Menu("Do you want do delete all NN files?", new string[] { "Yes", "No" });
            menu.SetSelected(1);
            if (menu.Show(new Vector2Int(4, 2)) == 0)
            {
                string[] files = Directory.GetFiles("E:\\NeuralNetwork\\");
                for (int i = 0; i < files.Length; i++)
                    SFile.Delete(files[i]);
                Console.WriteLine($"Deleted {files.Length} file{(files.Length != 1 ? "s" : "")}");
                Wait(800);
            }
            Console.Clear();
        }

        private static void Zoom()
        {
            MousePoint mp = MouseOperations.GetCursorPosition();
            MouseOperations.SetCursorPosition(360, 96);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            Wait(200);
            MouseOperations.SetCursorPosition(85, 150);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            Wait(200);
            MouseOperations.SetCursorPosition(310, 70);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            MouseOperations.SetCursorPosition(mp);
        }

        private static void New(InputSimulator IS)
        {
            MousePoint mp = MouseOperations.GetCursorPosition();
            MouseOperations.SetCursorPosition(85, 36);
            Wait(100);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10, false);//!saved);
            Wait(500);
            MouseOperations.SetCursorPosition(100, 60);
            Wait(100);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            Wait(500);
            IS.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
            IS.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
            IS.Keyboard.KeyDown(VirtualKeyCode.RETURN);
            IS.Keyboard.KeyUp(VirtualKeyCode.RETURN);
            MouseOperations.SetCursorPosition(mp);
        }

        private static void Save(int imgNumb, InputSimulator IS)
        {
            MousePoint mp = MouseOperations.GetCursorPosition();
            MouseOperations.SetCursorPosition(85, 36);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10, false);
            MouseOperations.SetCursorPosition(100, 200);
            Wait(500);
            MouseOperations.MouseClick(MouseOperations.MouseButton.Left, 10);
            Wait(1000);
            IS.Keyboard.KeyDown(VirtualKeyCode.BACK);
            IS.Keyboard.KeyDown(VirtualKeyCode.VK_I);
            IS.Keyboard.KeyDown(VirtualKeyCode.VK_M);
            IS.Keyboard.KeyDown(VirtualKeyCode.VK_G);
            PressNumber(imgNumb, IS);
            IS.Keyboard.KeyDown(VirtualKeyCode.RETURN);
            MouseOperations.SetCursorPosition(mp);
        }

        private static void PressNumber(int numb, InputSimulator IS)
        {
            string number = numb.ToString();

            for (int i = 0; i < number.Length; i++)
            {
                char ch = number[i];
                if (ch == '0')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD0);
                else if (ch == '1')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD1);
                else if (ch == '2')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD2);
                else if (ch == '3')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD3);
                else if (ch == '4')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD4);
                else if (ch == '5')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD5);
                else if (ch == '6')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD6);
                else if (ch == '7')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD7);
                else if (ch == '8')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD8);
                else if (ch == '9')
                    IS.Keyboard.KeyDown(VirtualKeyCode.NUMPAD9);
            }
        }

        private static Process GetProcess()
        {
            Process[] process = Process.GetProcessesByName("mspaint");
            if (process.Length > 0)
                return process[0];
            else
            {
                Console.WriteLine("Open MSPaint, than press any key to continue");
                Console.ReadKey(true);
                return GetProcess();
            }
        }
    }
}
