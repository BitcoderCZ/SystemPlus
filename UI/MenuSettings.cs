using System;
using System.Text;
using SystemPlus.Extensions;
using SystemPlus.Vectors;
using static SystemPlus.Extensions.GeneralExtensions;

namespace SystemPlus.UI
{
    public sealed class MenuSettings : IMenu<MenuSettings.STATUS>
    {
        public class STATUS
        {
            private byte s;

            public static readonly STATUS OK = new STATUS(1);
            public static readonly STATUS CANCEL = new STATUS(2);
            public static readonly STATUS BUTTON_EXIT = new STATUS(3);

            private STATUS(byte _s) { s = _s; }

            public static bool operator ==(STATUS a, STATUS b) => a.s == b.s;
            public static bool operator !=(STATUS a, STATUS b) => a.s != b.s;

            public override bool Equals(object obj)
            {
                if (obj is STATUS ss)
                    return ss == this;
                else
                    return false;
            }

            public override int GetHashCode() => s;
        }

        private int selected;
        private IMenuSettingsItem[] options;
        private string title;

        private Vector2Int rp;

        public IMenuSettingsItem[] lastValues;

        public MenuSettings(string _title, IMenuSettingsItem[] _options)
        {
            rp = Vector2Int.Zero;
            selected = 0;
            title = _title;

            options = new IMenuSettingsItem[_options.Length + 2];
            Array.Copy(_options, 0, options, 0, _options.Length);

            options[options.Length - 2] = new MSILabel("\n" + "Cancel".ToBIG());
            options[options.Length - 1] = new MSILabel("Ok".ToBIG());

            for (int i = 0; i < options.Length; i++)
                options[i].renderRequest = () => { Render(rp); };

            lastValues = new IMenuSettingsItem[_options.Length];
        }

        public STATUS Show(Vector2Int pos)
        {
            rp = pos;
            Console.OutputEncoding = Encoding.Unicode;
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
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter && (selected == options.Length - 1 || selected == options.Length - 2))
                {
                    if (selected == options.Length - 1)
                        Array.Copy(options, 0, lastValues, 0, lastValues.Length);

                    return selected == options.Length - 1 ? STATUS.OK : STATUS.CANCEL;
                }
                else
                {
                    if (options[selected] is MSIButton btn)
                    {
                        MSIButton.EXIT_CODE code = (MSIButton.EXIT_CODE)btn.GetValue();
                        if (code == MSIButton.EXIT_CODE.Exit)
                            return STATUS.BUTTON_EXIT;
                    }
                    else
                        options[selected].OnKeyPress(arrow);
                }

                selected = MathPlus.Clamp(selected, 0, options.Length);
            }
        }

        public STATUS Show(Vector2Int pos, Image image)
        {
            rp = pos;
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
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter && (selected == options.Length - 1 || selected == options.Length - 2))
                {
                    if (selected == options.Length - 1)
                        Array.Copy(options, 0, lastValues, 0, lastValues.Length);

                    return selected == options.Length - 1 ? STATUS.OK : STATUS.CANCEL;
                }
                else
                    options[selected].OnKeyPress(arrow);

                selected = MathPlus.Clamp(selected, 0, options.Length);
            }
        }

        public STATUS Show(Vector2Int pos, ConsoleImage image)
        {
            rp = pos;
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
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        string render = options[i].Render();
                        string[] lines = render.GetLines();

                        if (lines.Length < 2)
                            Console.Write(options[i].Render());
                        else
                            for (int j = 0; j < lines.Length; j++)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write(lines[j] + "\n");
                            }
                        Console.ResetColor();
                        Console.Write('\n');
                    }
                }

                Console.ResetColor();

                Arrows arrow = GetArrow();

                if (arrow == Arrows.Up && selected > 0)
                    selected--;
                else if (arrow == Arrows.Down && selected < options.Length - 1)
                    selected++;
                else if (arrow == Arrows.Enter && (selected == options.Length - 1 || selected == options.Length - 2))
                {
                    if (selected == options.Length - 1)
                        Array.Copy(options, 0, lastValues, 0, lastValues.Length);

                    return selected == options.Length - 1 ? STATUS.OK : STATUS.CANCEL;
                }
                else
                    options[selected].OnKeyPress(arrow);

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
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
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
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
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
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    string render = options[i].Render();
                    string[] lines = render.GetLines();

                    if (lines.Length < 2)
                        Console.Write(options[i].Render());
                    else
                        for (int j = 0; j < lines.Length; j++)
                        {
                            Console.CursorLeft = pos.x;
                            Console.Write(lines[j] + "\n");
                        }
                    Console.ResetColor();
                    Console.Write('\n');
                }
            }

            Console.ResetColor();
        }

        public void SetSelected(int _selected) => selected = MathPlus.Clamp(_selected, 0, options.Length);
    }

    public delegate void RenderRequest();

    public interface IMenuSettingsItem
    {
        public string Name { get; set; }

        public abstract void SetDefault();

        public abstract void OnKeyPress(Arrows keyInfo);

        public abstract string Render();

        public object GetValue();

        public RenderRequest renderRequest { get; set; }
    }

    public abstract class MenuSettingsItem<TValue> : IMenuSettingsItem
    {
        public abstract string Name { get; set; }
        public TValue Value { get; protected set; }

        public abstract void SetDefault();

        public abstract void OnKeyPress(Arrows keyInfo);

        public abstract string Render();

        public object GetValue() => Value;

        public RenderRequest renderRequest { get; set; }
    }

    public class MSILabel : MenuSettingsItem<string>
    {
        public override string Name { get; set; }

        public MSILabel(string name)
        {
            Name = name;
            Value = Name;
        }

        public override void SetDefault() { }

        public override void OnKeyPress(Arrows keyInfo) { }

        public override string Render() => Name;
    }

    public class MSIBool : MenuSettingsItem<bool>
    {
        public override string Name { get; set; }

        public byte dispType;

        public const byte TrueFalse = 1;
        public const byte OnOff = 2;
        public const byte YesNo = 3;
        public const byte Checkmark = 4;

        private bool defaultValue;

        public MSIBool(string name, bool _defaultValue)
        {
            Name = name;
            defaultValue = _defaultValue;
            Value = defaultValue;
            dispType = TrueFalse;
        }

        public override void SetDefault() => Value = defaultValue;

        public override void OnKeyPress(Arrows keyInfo)
        {
            switch (keyInfo)
            {
                case Arrows.Left:
                case Arrows.Rigth:
                case Arrows.Enter:
                    Value = !Value;
                    break;
                default:
                    break;
            }
        }

        private string getVal()
        {
            switch (dispType)
            {
                case TrueFalse:
                    return (Value == true) ? "True " : "False";
                case OnOff:
                    return (Value == true) ? "On " : "Off";
                case YesNo:
                    return (Value == true) ? "Yes" : "No ";
                case Checkmark:
                    return (Value == true) ? "☑" : "☐";
                default:
                    return null;
            }
        }

        public override string Render() => Name + ": " + getVal();
    }

    public class MSIButton : IMenuSettingsItem
    {
        public string Name { get; set; }

        public RenderRequest renderRequest { get; set; }

        public enum EXIT_CODE : byte
        {
            None = 0,
            Exit = 1,
        }

        private Func<MSIButton, EXIT_CODE> onPress;

        public MSIButton(string name, Func<MSIButton, EXIT_CODE> _onPress)
        {
            Name = name;
            onPress = _onPress;
        }

        public void SetDefault() { }

        public void OnKeyPress(Arrows keyInfo) { }

        public object GetValue() => onPress.Invoke(this);

        public string Render() => Name;
    }

    public class MSIIntValue : MenuSettingsItem<int>
    {
        public override string Name { get; set; }

        public byte dispType;

        public const byte Normal = 1;
        public const byte MinMax = 2;

        private int defaultValue;

        private int lowBounds;

        private int higthBounds;

        public MSIIntValue(string name, int _defaultValue, int _lowBounds, int _higthBounds)
        {
            Name = name;
            defaultValue = _defaultValue;
            lowBounds = _lowBounds;
            higthBounds = _higthBounds;

            if (defaultValue < lowBounds)
                defaultValue = lowBounds;
            else if (defaultValue > higthBounds)
                defaultValue = higthBounds;

            Value = defaultValue;
            dispType = Normal;

            gettingNumber = false;
            gettingString = "";
        }

        public override void SetDefault() => Value = defaultValue;

        public override void OnKeyPress(Arrows keyInfo)
        {
            switch (keyInfo)
            {
                case Arrows.Left:
                    if (Value > lowBounds)
                    {
                        int l = Value.ToString().Length;
                        Value--;
                        if (Value.ToString().Length < l)
                        {
                            gettingNumber = true;
                            gettingString = new string(' ', higthBounds.ToString().Length);
                            renderRequest?.Invoke();
                            gettingNumber = false;
                        }
                    }
                    break;
                case Arrows.Rigth:
                    if (Value < higthBounds)
                        Value++;
                    break;
                case Arrows.Enter:
                    GetNumber();
                    break;
                default:
                    break;
            }
        }

        private void GetNumber()
        {
            gettingNumber = true;

            gettingString = new string(' ', higthBounds.ToString().Length);
            renderRequest?.Invoke();

            Console.CursorVisible = true;

            gettingString = "";



            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (char.IsDigit(key.KeyChar) || key.KeyChar == '-')
                    gettingString += key.KeyChar;
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (gettingString.Length > 1)
                    {
                        gettingString = gettingString.Substring(0, gettingString.Length - 1);
                        gettingString += ' ';
                        renderRequest?.Invoke();
                        gettingString = gettingString.Substring(0, gettingString.Length - 1);
                    }
                    else if (gettingString.Length == 1)
                        gettingString = "";
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        int numbI = int.Parse(gettingString);

                        if (numbI < lowBounds)
                            numbI = lowBounds;
                        else if (numbI > higthBounds)
                            numbI = higthBounds;

                        Value = numbI;
                    }
                    catch { Value = lowBounds; }
                    break;
                }
                else if (key.Key == ConsoleKey.Escape)
                    break;

                renderRequest?.Invoke();
            }

            Console.CursorVisible = false;

            gettingNumber = false;

            Console.Clear();

            renderRequest?.Invoke();
        }

        private bool gettingNumber;
        private string gettingString;

        private string getVal()
        {
            const string btw = " / ";
            switch (dispType)
            {
                case Normal:
                    if (gettingNumber)
                        return string.IsNullOrEmpty(gettingString) ? "0" : gettingString;
                    else
                        return Value.ToString();
                case MinMax:
                    if (gettingNumber)
                        return lowBounds + btw + (string.IsNullOrEmpty(gettingString) ? "0" : gettingString) + btw + higthBounds + "    ";
                    else
                        return lowBounds + btw + Value.ToString() + btw + higthBounds + "    ";
                default:
                    return null;
            }
        }

        public override string Render() => Name + ": " + getVal();
    }

    public class MSIIntSlider : MenuSettingsItem<int>
    {
        public override string Name { get; set; }

        public int sliderLength;

        public byte dispType;

        public const byte Normal = 1;
        public const byte MinMax = 2;

        private int defaultValue;

        private int lowBounds;

        private int higthBounds;

        public MSIIntSlider(string name, int _defaultValue, int _lowBounds, int _higthBounds)
        {
            Name = name;
            defaultValue = _defaultValue;
            lowBounds = _lowBounds;
            higthBounds = _higthBounds;

            if (defaultValue < lowBounds)
                defaultValue = lowBounds;
            else if (defaultValue > higthBounds)
                defaultValue = higthBounds;

            Value = defaultValue;
            dispType = Normal;

            sliderLength = 10;

            gettingNumber = false;
            gettingString = "";
        }

        public override void SetDefault() => Value = defaultValue;

        public override void OnKeyPress(Arrows keyInfo)
        {
            switch (keyInfo)
            {
                case Arrows.Left:
                    if (Value > lowBounds)
                    {
                        int l = Value.ToString().Length;
                        Value--;
                        if (Value.ToString().Length < l)
                        {
                            gettingNumber = true;
                            gettingString = new string(' ', higthBounds.ToString().Length + lowBounds.ToString().Length + l.ToString().Length);
                            renderRequest?.Invoke();
                            gettingNumber = false;
                        }
                    }
                    break;
                case Arrows.Rigth:
                    if (Value < higthBounds)
                        Value++;
                    break;
                case Arrows.Enter:
                    GetNumber();
                    break;
                default:
                    break;
            }
        }

        private void GetNumber()
        {
            gettingNumber = true;

            gettingString = new string(' ', higthBounds.ToString().Length);
            renderRequest?.Invoke();

            Console.CursorVisible = true;

            gettingString = "";



            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (char.IsDigit(key.KeyChar) || key.KeyChar == '-')
                    gettingString += key.KeyChar;
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (gettingString.Length > 1)
                    {
                        gettingString = gettingString.Substring(0, gettingString.Length - 1);
                        gettingString += ' ';
                        renderRequest?.Invoke();
                        gettingString = gettingString.Substring(0, gettingString.Length - 1);
                    }
                    else if (gettingString.Length == 1)
                        gettingString = "";
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        int numbI = int.Parse(gettingString);

                        if (numbI < lowBounds)
                            numbI = lowBounds;
                        else if (numbI > higthBounds)
                            numbI = higthBounds;

                        Value = numbI;
                    }
                    catch { Value = lowBounds; }
                    break;
                }
                else if (key.Key == ConsoleKey.Escape)
                    break;

                renderRequest?.Invoke();
            }

            Console.CursorVisible = false;

            gettingNumber = false;

            Console.Clear();

            renderRequest?.Invoke();
        }

        private bool gettingNumber;
        private string gettingString;

        private string getVal()
        {
            const string btw = " / ";
            switch (dispType)
            {
                case Normal:
                    if (gettingNumber)
                        return string.IsNullOrEmpty(gettingString) ? "0" : gettingString;
                    else
                        return Value.ToString();
                case MinMax:
                    if (gettingNumber)
                        return lowBounds + btw + (string.IsNullOrEmpty(gettingString) ? "0" : gettingString) + btw + higthBounds + "    ";
                    else
                        return lowBounds + btw + Value.ToString() + btw + higthBounds + "    ";
                default:
                    return null;
            }
        }

        private string getSlider()
        {
            int vvv = 0;

            if (gettingNumber)
            {
                if (string.IsNullOrEmpty(gettingString) || gettingString == "  ")
                    vvv = lowBounds;
                else
                {
                    try
                    {
                        vvv = int.Parse(gettingString);
                    }
                    catch
                    {
                        return "X" + new string(' ', sliderLength);
                    }
                }
            }
            else
                vvv = Value;

            string ss = new string('-', sliderLength);//■▪
            StringBuilder sb = new StringBuilder(ss);
            int v = MathPlus.Clamp(
                MathPlus.RoundToInt(((float)(vvv - lowBounds) / (float)(higthBounds - lowBounds)) * (float)sliderLength),
                0,
                ss.Length - 1
                );

            sb[v] = '■';

            return sb.ToString();
        }

        public override string Render() => Name + ": " + getVal() + " " + getSlider();
    }
}
