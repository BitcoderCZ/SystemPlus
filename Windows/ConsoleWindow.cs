using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Systemplus.Inputs;
using SystemPlus.Extensions;
using SystemPlus.Font;
using SystemPlus.Utils;
using SystemPlus.Vectors;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace SystemPlus.Windows
{
    public sealed class ConsoleWindow : Window, IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);


        private static Thread updateThread;

        private static List<ConsoleWindow> consolesToUpdate = new List<ConsoleWindow>();


        private new int Width
        {
            get => (int)base.Width;
            set
            {
                base.Width = value;
                SetRes();
            }
        }
        private new int Height
        {
            get => (int)base.Height;
            set
            {
                base.Height = value;
                SetRes();
            }
        }

        public double resolution { get; private set; }

        public Vector2Int Position { get => new Vector2Int(MathPlus.RoundToInt(Left), MathPlus.RoundToInt(Top)); set { Left = Position.x; Top = Position.y; } }

        private IntPtr HostHandle { get; set; }

        private Size HostSize { get; set; }

        private Size BufferSize { get; set; }

        private Graphics GraphicsHost { get; set; }

        private IntPtr GraphicsHostDeviceContext { get; set; }

        private BufferedGraphics BufferedGraphics { get; set; }

        private DirectBitmap Buffer;
        private DirectBitmap FronterBuffer;

        private bool drawing = false;

        private TaskFactory factory;

        private EngineInput Input { get; set; }

        public bool setUp { get; private set; } = false;

        private static int fontID = (Environment.CurrentDirectory + "\\consolas.ttf").GetHashCode();


        public int textSize { get; set; } = 30;

        public int bufferHeightDisplayed { get; set; } = 16;

        public int bufferOffset { get; set; } = 0;

        public CPColor ForegroundColor { get; set; } = CPColor.Gray;

        public Vector2Int CursorPos { get; set; } = Vector2Int.Zero;

        public Size CharBufferSize { get; private set; } = Size.Empty;


        private readonly int[] colors = new int[] 
        {
             Color.Black.ToArgb(),   
             Color.DarkBlue.ToArgb(),   
             Color.DarkGreen.ToArgb(),   
             Color.DarkCyan.ToArgb(),   
             Color.DarkRed.ToArgb(),   
             Color.DarkMagenta.ToArgb(),   
             Color.Orange.ToArgb(),   
             Color.Gray.ToArgb(),   
             Color.DarkGray.ToArgb(),   
             Color.Blue.ToArgb(),   
             Color.Green.ToArgb(),   
             Color.Cyan.ToArgb(),   
             Color.Red.ToArgb(),   
             Color.Magenta.ToArgb(),   
             Color.Yellow.ToArgb(),   
             Color.White.ToArgb(),   
        };

        private CPChar[][] charBuffer;

        private Size charSize;


        private Queue<(Key key, Modifiers modifiers)> keysQueued;

        public int KeysQueued => keysQueued.Count;

        public void ClearQueuedKey() => keysQueued.Clear();

        private int keyRequests;

        public IKeyboardLayout KeyboardLayout { get; set; }


        private ConsoleWindow()
        {
            charSize = new Size();
            keysQueued = new Queue<(Key key, Modifiers modifiers)>();
            keyRequests = 0;
        }

        [STAThread]
        private void Init(System.Windows.Forms.Panel hostControl)
        {
            FontLibrary.GetOrCreateFromFile(Environment.CurrentDirectory + "\\consolas.ttf");
            CreateCharBuffer(new Size(40, 100));

            HostHandle = hostControl.Handle();
            Input = new EngineInput(hostControl);
            Input.Hook();

            HostSize = Input.Size;
            BufferSize = Input.Size;

            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateSurface(Input.Size);
            CreateBuffers(BufferSize);

            factory = new TaskFactory();

            setUp = true;

            consolesToUpdate.Add(this);

            if (updateThread == null)
            {
                updateThread = new Thread(new ThreadStart(() =>
                {
                    Stopwatch frameWatch = new Stopwatch();
                    while (!Dispatcher.HasShutdownStarted)
                    {
                        frameWatch.Start();
                        for (int i = 0; i < consolesToUpdate.Count; i++)
                            consolesToUpdate[i].Update();
                        frameWatch.Stop();
                        Thread.Sleep(Math.Max(100 - (int)(uint)frameWatch.ElapsedMilliseconds, 0));
                        frameWatch.Reset();
                    }
                }));
                updateThread.Start();
            }
        }

        public static ConsoleWindow Create(string windowTitle)
        {
            ConsoleWindow window = new ConsoleWindow()
            {
                Title = windowTitle,
                ResizeMode = ResizeMode.CanMinimize,
            };

            System.Windows.Forms.Panel hostControl = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
            };

            void EnsureFocus(System.Windows.Forms.Control control)
            {
                if (!control.Focused)
                {
                    control.Focus();
                }
            }

            hostControl.MouseEnter += (sender, args) => EnsureFocus(hostControl);
            hostControl.MouseClick += (sender, args) => EnsureFocus(hostControl);

            System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = hostControl,
            };

            window.Content = windowsFormsHost;

            window.Closed += (sender, args) => Environment.Exit(0);

            window.Show();
            window.Init(hostControl);

            return window;
        }

        private void Update()
        {
            /*Stopwatch frameWatch = new Stopwatch();
            while (!Dispatcher.HasShutdownStarted)
            {
                frameWatch.Start();*/

                // Input
                Input.Update();
                for (int i = 0; i < Input.keys.Length; i++)
                {
                    if ((Input.keys[i].pressedDown == true || Input.keys[i].pressedDuration > 30) && keyRequests > 0)
                        keysQueued.Enqueue((Input.keys[i].Key, Input.KeyModifiers[Input.keys[i].Key]));
                }

                // Render
                Buffer.Clear(Color.Black.ToArgb());
                for (int i = bufferOffset; i < Math.Min(bufferOffset + bufferHeightDisplayed, charBuffer.Length); i++)
                    FontRender.RenderConsole(FontLibrary.Get(fontID), Buffer, charBuffer[i], colors, 0, i * charSize.Height, textSize);
                Render();
                //Event
                //System.Windows.Forms.Application.DoEvents();

               /* frameWatch.Stop();

                Thread.Sleep(Math.Max(1000 - (int)(uint)frameWatch.ElapsedMilliseconds, 0));
            }
            Dispose();*/
        }

        private void CreateSurface(Size size)
        {
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHostDeviceContext, new Rectangle(Point.Empty, size));
            BufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void CreateBuffers(Size size)
        {
            Buffer = new DirectBitmap(size);
            FronterBuffer = new DirectBitmap(size);
        }

        private void Render()
        {
            /*if (!drawing)
            {*/
                drawing = true;
                Buffer.CopyTo(FronterBuffer);
                drawToScreen();//factory.StartNew(drawToScreen);
            Console.WriteLine("Ren");
            //}
        }

        private void drawToScreen()
        {
            BufferedGraphics?.Graphics.DrawImage(
                FronterBuffer.Bitmap,
                new RectangleF(PointF.Empty, HostSize),
                new RectangleF(new PointF(-0.5f, -0.5f), BufferSize),
                GraphicsUnit.Pixel);

            // swap buffers
            BufferedGraphics?.Render(GraphicsHostDeviceContext);
            drawing = false;
        }


        /// <summary>
        /// Creates character buffer
        /// </summary>
        /// <param name="size">Size in characters</param>
        public void CreateCharBuffer(Size size)
        {
            charBuffer = new CPChar[size.Height][];
            Parallel.For(0, size.Height, (int i) => charBuffer[i] = new CPChar[size.Width]);

            charSize = new Size(0, 0);

            for (byte i = 32; i < 127; i++)
            {
                Size _charSize = FontRender.GetTextSize(FontLibrary.Get(fontID), ((char)i).ToString(), textSize, 2);
                if (_charSize.Width > charSize.Width)
                    charSize.Width = _charSize.Width;
                if (_charSize.Height > charSize.Height)
                    charSize.Height = _charSize.Height;
            }

            Width = (charSize.Width + 2) * size.Width;
            Height = charSize.Height * Math.Min(bufferHeightDisplayed, size.Height) + charSize.Height;

            CharBufferSize = size;

            SetRes();
        }

        public void Write(string value)
        {
            if (CursorPos.y >= charBuffer.Length)
                CursorPos.y = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '\n' || CursorPos.x >= CharBufferSize.Width)
                {
                    CursorPos.y++;
                    if (CursorPos.y >= charBuffer.Length)
                        CursorPos.y = 0;
                    CursorPos.x = 0;

                    charBuffer[CursorPos.y][CursorPos.x] = new CPChar(value[i], (byte)ForegroundColor);
                } else
                {
                    charBuffer[CursorPos.y][CursorPos.x] = new CPChar(value[i], (byte)ForegroundColor);

                    CursorPos.x++;
                }
            }
        }
        public void Write(object value) => Write(value.ToString());

        public void WriteLine(string value) => Write(value + "\n");
        public void WriteLine(object value) => WriteLine(value.ToString());

        private object queueReadLock = new object();

        private object writeFromReadLock = new object();

        public KeyInfo Read(bool display)
        {
            keyRequests++;

            (Key key, Modifiers modifiers) KeyPlusMod;

            while (true)
            {
                lock (queueReadLock) {
                    if (keysQueued.Count > 0)
                    {
                        KeyPlusMod = keysQueued.Dequeue();
                        break;
                    }    
                }

                Thread.Sleep(0);
            }

            keyRequests--;

            if (display)
                lock (writeFromReadLock)
                    Write(KeyboardLayout.Map(KeyPlusMod.key, KeyPlusMod.modifiers));

            return new KeyInfo(KeyPlusMod.key, KeyPlusMod.modifiers, KeyboardLayout.Map(KeyPlusMod.key, KeyPlusMod.modifiers));
        }


        public void SetColor(CPColor color, Color newColor)
            => colors[(byte)color] = newColor.ToArgb();

        private byte CreateColor(CPColor fg, CPColor bg) => (byte)((byte)fg | ((byte)bg << 4));

        private void SetRes() => resolution = (double)Width / (double)Height;

        private Vector2Int RelativeToPixel(Vector2D value)
        {
            value.Y = -value.Y;
            value.Y *= resolution;
            value++;
            value /= 2d;
            value.X *= (double)Width;
            value.Y *= (double)Height;
            return value.ToVector2Int();
        }

        private int RelativeToPixelX(double value)
        {
            value++;
            value /= 2;
            value *= (double)Width;
            return MathPlus.RoundToInt(value);
        }

        private int RelativeToPixelY(double value)
        {
            value *= resolution;
            value++;
            value /= 2;
            value *= (double)Height;
            return MathPlus.RoundToInt(value);
        }

        public void Dispose()
        {
            if (consolesToUpdate.Contains(this))
                consolesToUpdate.Remove(this);
            Input?.UnHook();
            Input?.Dispose();
            Input = default;
            HostHandle = default;
            BufferedGraphics?.Dispose();
            BufferedGraphics = default;
            GraphicsHost?.Dispose();
            GraphicsHost = default;
            FronterBuffer?.Dispose();
            FronterBuffer = default;
            Buffer?.Dispose();
            Buffer = default;
            factory = default;
        }
    }

    public readonly struct KeyInfo
    {
        public readonly Key Key;
        public readonly Modifiers Modifiers;
        public readonly char Char;

        public KeyInfo(Key _key, Modifiers _modifiers, char _char)
        {
            Key = _key;
            Modifiers = _modifiers;
            Char = _char;
        }
    }

    public readonly struct CPChar
    {
        public readonly char Char;
        public readonly byte Color;

        public CPChar(char _char, byte _color)
        {
            Char = _char;
            Color = _color;
        }
    }

    public enum CPColor : byte
    {
        //
        // Summary:
        //     The color black.
        Black = 0,
        //
        // Summary:
        //     The color dark blue.
        DarkBlue = 1,
        //
        // Summary:
        //     The color dark green.
        DarkGreen = 2,
        //
        // Summary:
        //     The color dark cyan (dark blue-green).
        DarkCyan = 3,
        //
        // Summary:
        //     The color dark red.
        DarkRed = 4,
        //
        // Summary:
        //     The color dark magenta (dark purplish-red).
        DarkMagenta = 5,
        //
        // Summary:
        //     The color orange.
        Orange = 6,
        //
        // Summary:
        //     The color gray.
        Gray = 7,
        //
        // Summary:
        //     The color dark gray.
        DarkGray = 8,
        //
        // Summary:
        //     The color blue.
        Blue = 9,
        //
        // Summary:
        //     The color green.
        Green = 10,
        //
        // Summary:
        //     The color cyan (blue-green).
        Cyan = 11,
        //
        // Summary:
        //     The color red.
        Red = 12,
        //
        // Summary:
        //     The color magenta (purplish-red).
        Magenta = 13,
        //
        // Summary:
        //     The color yellow.
        Yellow = 14,
        //
        // Summary:
        //     The color white.
        White = 15
    }
}
