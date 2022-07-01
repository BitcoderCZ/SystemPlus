using System;
//using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using SystemPlus.Extensions;
using SystemPlus.GameEngines;
using SystemPlus.Vectors;

namespace SystemPlus
{

    internal static class Program
    {
        //public static Image current;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        static Random rand;

        /*private static List<NeuralNetworkTest> tests = new List<NeuralNetworkTest>()
        {
            new NeuralNetworkTest(new List<double> { 0, 0, 0 }, new List<double> { 0 }),
            new NeuralNetworkTest(new List<double> { 0, 0, 1 }, new List<double> { 1 }),
            new NeuralNetworkTest(new List<double> { 0, 1, 0 }, new List<double> { 0 }),
            new NeuralNetworkTest(new List<double> { 0, 1, 1 }, new List<double> { 0 }),
            new NeuralNetworkTest(new List<double> { 1, 0, 0 }, new List<double> { 0 }),
            new NeuralNetworkTest(new List<double> { 1, 0, 1 }, new List<double> { 0 }),
            new NeuralNetworkTest(new List<double> { 1, 1, 0 }, new List<double> { 1 }),
            new NeuralNetworkTest(new List<double> { 1, 1, 1 }, new List<double> { 0 }),
        };*/

        static void Main(string[] args)
        {
            ConsoleExtensions.SetFontSize(40);
            /* GeneralExtensions.Wait(500);
             GeneralExtensions.MaxScreen();
             GeneralExtensions.Wait(500);

             NeuralNetwork net = new NeuralNetwork(new int[] { 3, 25, 25, 1 });

             for (int i = 0; i < 10000; i++)
             {
                 net.FeedForward(new float[] { 0, 0, 0 });
                 net.BackProp(new float[] { 0 });
                 net.FeedForward(new float[] { 0, 0, 1 });
                 net.BackProp(new float[] { 1 });
                 net.FeedForward(new float[] { 0, 1, 0 });
                 net.BackProp(new float[] { 1 });
                 net.FeedForward(new float[] { 0, 1, 1 });
                 net.BackProp(new float[] { 0 });
                 net.FeedForward(new float[] { 1, 0, 0 });
                 net.BackProp(new float[] { 1 });
                 net.FeedForward(new float[] { 1, 0, 1 });
                 net.BackProp(new float[] { 0 });
                 net.FeedForward(new float[] { 1, 1, 0 });
                 net.BackProp(new float[] { 0 });
                 net.FeedForward(new float[] { 1, 1, 1 });
                 net.BackProp(new float[] { 1 });
             }
             while (true)
             {
                 int i1 = int.Parse(Console.ReadLine());
                 int i2 = int.Parse(Console.ReadLine());
                 int i3 = int.Parse(Console.ReadLine());


             }*/
            /*Game game = new Game();
            game.GameStart();*/

            /*if (args.Length > 0 && args[0] != null)
            {
                Console.WriteLine("Path: " + args[0]);
            }
            else
            {
                Console.WriteLine("Normal");
            }*/

            //img.SaveToLocation("C:\\Users\\Tomas\\Pictures\\", "ta cena.CATMG");
            /*Image img = Image.Create("C:\\Users\\Tomas\\Pictures\\Rychlost internetu.PNG");
            img = img.Resize((int)((float)img.Width * 2f), (int)((float)img.Height * 2f));
            img.Save("C:\\Users\\Tomas\\Pictures\\", "Rychlost internetu.png");*/

            /*System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Arial");
            System.Drawing.Font font = new System.Drawing.Font(fontFamily, 30f);
            Font.GetGlyphShape(font, 'a');*/

            Console.ReadKey();

            /*Console.OutputEncoding = System.Text.Encoding.UTF8;

            ConsoleHelper.SetFontSize(30);

            GeneralExtensions.Wait(500);

            Game_2048 gm = new Game_2048();

            while (true)
            {
                Console.SetCursorPosition(0, 5);
                ColoredString cs = new ColoredString(gm.DisplayColored);
                //Console.BackgroundColor = ConsoleColor.White;
                cs.Print();
                if (gm.SetInput(gm.ToInput(Console.ReadKey(true).Key)))
                    break;
            }*/


            /*tests = new List<NeuralNetworkTest>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y <= x; y++)
                {
                    double[] exres = new double[10];
                    exres[x] = 1d;
                    tests.Add(new NeuralNetworkTest(new List<double>() { y, x - y }, exres.ToList()));
                }
            }


            NeuralNetwork nw;// ; = new NeuralNetwork(2, new int[] { 20, 20, 20 }, 10);
            nw = NeuralNetwork.TrainMostLikely(tests, new NuralNetworkSettings(2, new int[] { 20, 50, 30, 30, 30, 20, 10 }, 10), 500, 50);
            Console.WriteLine("Done");
            nw.PredictMostLikely(tests).Print();
            while (true)
            {
                int a = int.Parse(Console.ReadLine());
                int b = int.Parse(Console.ReadLine());
                double[] res = nw.Predict(new double[] { a, b }, true);
                Vector2[] resI = new Vector2[res.Length];
                for (int i = 0; i < res.Length; i++)
                    resI[i] = new Vector2(i, (float)res[i]);
                resI = resI.ToList().OrderBy(v => v.y).ToArray();
                /*Console.WriteLine(res[0] + " " +
                    res[1] + " " +
                    res[2] + " " +
                    res[3] + " " +
                    res[4] + " " +
                    res[5] + " " +
                    res[6] + " " +
                    res[7] + " " +
                    res[8] + " " +
                    res[9] + " ");*/
            /* Console.WriteLine(resI.Last().x + "\n");
         }*/
            /*NeuralNetworkNew net = new NeuralNetworkNew(new int[] { 3, 25, 25, 25, 25, 25, 1}, 5.333d);
            net.Train(tests, 50000);
            Console.WriteLine("Trained!");
            foreach (NeuralNetworkTest test in tests)
                Console.WriteLine(net.FeedForward(test.inputs.ToArray())[0]);
            while (true)
            {
                int a = int.Parse(Console.ReadLine());
                int b = int.Parse(Console.ReadLine());
                int c = int.Parse(Console.ReadLine());
                Console.WriteLine(net.FeedForward(new double[] { a, b, c })[0]);
            }
            Console.ReadKey();*/
        }

        /*[STAThread]
        static void Main(string[] args)
        {
            int i = int.Parse(Console.ReadLine());
            Console.WriteLine((object)Convert.ToByte(i));
            Console.WriteLine();
            Console.WriteLine("{0,25}{1,30}", i,
                BitConverter.ToString(BitConverter.GetBytes(i)));
            Console.WriteLine();
            Main(null);
            /*bool auto = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.Title = "SystemPlus";
            Console.Clear();
            Console.Write("Compression Mode: ");
            int X = 1;
            try
            {
                string readed = Console.ReadLine();
                if (readed.ToLower() == "a")
                    auto = true;
                else if (readed.ToLower() == "/stop")
                    return;
                else
                    X = int.Parse(readed);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }
            Console.Clear();
            if(auto) 
                Console.WriteLine("Compression Mode: A");
            else
                Console.WriteLine("Compression Mode: " + X + " times");
            Extensions.Wait(1500);
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open Image",
                Filter = "Image|*.png;*.jpg"
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("Selected: " + Path.GetFileName(dialog.FileName));
                    current = Image.FromFile(dialog.FileName);
                }
            }
            Extensions.Wait(1500);
            /*string[] OUT = current.Display(X, auto).Split(' ');

            Console.WriteLine("Width: " + OUT[0] + "  Height: " + OUT[1]);*/

        //ImageDisplayer.WriteImage(current);

        /*Application.EnableVisualStyles();

        var f = new Form();
        f.FormBorderStyle = FormBorderStyle.None;
        f.WindowState = FormWindowState.Maximized;
        PictureBox picture = new PictureBox();
        picture.Image = Image.ToBitmap(current);
        picture.Dock = DockStyle.Fill;
        picture.Show();
        f.Controls.Add(picture);
        f.Show();
        f.Refresh();

        Console.WriteLine();

        Console.ReadKey();
        f.Close();
        Main(null);*/

    }

    class Move : Component
    {
        public override void Update(float deltaTime)
        {
            //gameObject.Position.z -= 50f * deltaTime;
            //gameObject.Rotation.x += 1f * deltaTime;
            gameObject.Rotation.y += 0.5f * deltaTime;
            //gameObject.Rotation.z += 1f * deltaTime;
        }
    }

    class Game : GameEngine3D
    {
        public override StartInfo3D Start()
        {
            StartInfo3D si = new StartInfo3D("3D Game Test", 520, 480, 2);
            Clear = true;
            GameObject cube = new GameObject(Mesh.FromFile(
                File.ReadAllLines("E:\\Program Files\\VisualStudioProjects\\SystemPlus\\Data\\lowpolytree.obj"/*"E:\\untitled.obj"*/),
                Mesh.FileType.Obj, engine), engine);
            cube.Rotation = new Vector3(160, 0, 0);
            cube.Position.z = 1;
            cube.Scale = new Vector3(20, 20, 20);
            cube.AddComponent(new Move(), 0);
            gameObjects.Add(cube);
            return si;
        }

        public override void Update(float deltaTime, int miliseconds)
        {

        }

        public override void LateUpdate()
        {
            DrawSquare(0, 0, 518, 479, PIXEL.PIXEL_SOLID, CONSOLE_COLOR.Green);
        }
    }
}
