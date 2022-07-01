using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SystemPlus.Extensions;
using SystemPlus.Vectors;
using static SystemPlus.Extensions.GeneralExtensions;
using static SystemPlus.Extensions.IOExtensions;

namespace SystemPlus.UI
{
    public class MenuOpenFile : IMenu<MenuOpenFile.STATUS> // ADD MULTISELECT
    {
        private string title;

        public string Path;

        public string Selected;

        private int state;
        private LoadingAnim_1 anim;

        public class STATUS
        {
            private byte s;

            public static readonly STATUS OK = new STATUS(1);
            public static readonly STATUS CANCEL = new STATUS(2);

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

        public MenuOpenFile(string _title)
        {
            title = _title;
            Path = "C:\\";

            anim = new LoadingAnim_1();
        }

        public STATUS Show(Vector2Int pos)
        {
            string path = Path;
            int selected = 0;
            state = 0;
            anim.Reset();
            bool hasSizes = false;
            Selected = "";
            while (true)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(pos.x, pos.y);
                Console.ResetColor();

                for (int i = 0; i < title.GetLines().Length; i++)
                {
                    Console.CursorLeft = pos.x;
                    Console.Write(title.GetLines()[i] + "\n");
                }
                Console.Write("\n");
                Console.CursorLeft = pos.x;
                Console.WriteLine($"Path: {path}");

                if (path == "This PC:")
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();

                    for (int i = 0; i < drives.Length; i++)
                    {
                        Console.CursorLeft = pos.x;
                        DriveInfo cd = drives[i];

                        double total = MathPlus.Round(ChooseAppropriate(cd.TotalSize, Unit.B, out Unit unit), 2);
                        double used = MathPlus.Round(Convert(cd.TotalSize - cd.AvailableFreeSpace, Unit.B, unit), 2);

                        if (i == selected)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                        Console.WriteLine($"{cd.Name}  {used} {unit} / {total} {unit}");
                        if (i == selected)
                            Console.ResetColor();
                    }

                    Console.CursorTop++;
                    Console.CursorLeft = pos.x;
                    Console.Write("Selected: " + Selected + new string(' ', Console.WindowWidth));
                    Console.CursorLeft = pos.x;
                    ConsoleExtensions.Write("Cancel", selected == drives.Length);
                    Console.CursorTop++;
                    Console.CursorLeft = pos.x;
                    if (Selected != "")
                        ConsoleExtensions.Write("Ok", selected == drives.Length + 1);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("Ok");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Arrows key = GetArrow();

                    if (key == Arrows.Up && selected > 0)
                        selected--;
                    else if (key == Arrows.Down && selected < drives.Length + 1)
                        selected++;
                    else if (key == Arrows.Enter)
                    {
                        if (selected == drives.Length)
                            return STATUS.CANCEL;
                        else if (selected == drives.Length + 1 && Selected != "")
                            return STATUS.OK;
                        else if (selected != drives.Length + 1)
                        {
                            path = drives[selected].Name;
                            selected = 0;
                            hasSizes = false;
                            Console.Clear();
                        }
                    }
                }
                else
                {

                    if (selected == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    Console.CursorLeft = pos.x;
                    Console.WriteLine("Go Back");
                    if (selected == 0)
                        Console.ResetColor();

                    #region getDirsAndFiles

                    Console.CursorLeft = pos.x;

                    if (!hasSizes)
                    {
                        Thread thread = new Thread(new ThreadStart(() =>
                        {
                            while (state == 0)
                            {
                                Console.CursorLeft = pos.x;
                                Console.Write("Getting directory sizes " + anim.GetNextFrame() + " ");
                                Thread.Sleep(500);
                            }
                            state = 2;
                        }));

                        if (Console.OutputEncoding != Encoding.Unicode)
                            Console.OutputEncoding = Encoding.Unicode;

                        state = 0;

                        thread.Start();
                    }

                    string[] directoriesS = Directory.GetDirectories(path);
                    List<(string name, long size, string time)> directories = new List<(string name, long size, string time)>();

                    int dirLongestTime = 0;
                    int dirLongestName = 0;

                    for (int i = 0; i < directoriesS.Length; i++)
                    {
                        string time = Directory.GetCreationTime(directoriesS[i]).ToString();
                        if (time.Length > dirLongestTime)
                            dirLongestTime = time.Length;

                        string name = new DirectoryInfo(directoriesS[i]).Name + "\\";//System.IO.Path.GetDirectoryName(directoriesS[i] + "\\") + "\\";
                        if (name.Length > dirLongestName)
                            dirLongestName = name.Length;

                        directories.Add((
                            name,
                            Directory_GetSize(directoriesS[i], true, false, out string e),
                            time
                            ));
                    }

                    dirLongestTime += 3;
                    dirLongestName += 2;

                    string[] filesS = Directory.GetFiles(path);
                    List<(string name, long size, string time)> files = new List<(string name, long size, string time)>();

                    int fileLongestTime = 0;
                    int fileLongestName = 0;

                    for (int i = 0; i < filesS.Length; i++)
                    {
                        string time = Directory.GetCreationTime(filesS[i]).ToString();
                        if (time.Length > fileLongestTime)
                            fileLongestTime = time.Length;

                        string name = System.IO.Path.GetFileName(filesS[i]);
                        if (name.Length >= 4 && name.Substring(name.Length - 4, 4) == "SIZE") // skip saved directory sizes
                            continue;

                        if (name.Length > fileLongestName)
                            fileLongestName = name.Length;

                        files.Add((
                            name,
                            new FileInfo(filesS[i]).Length,
                            time
                            ));
                    }

                    fileLongestTime += 3;
                    fileLongestName += 2;

                    if (!hasSizes)
                    {
                        state = 1;
                        while (state == 1) { Thread.Sleep(1); }
                        state = 0;

                        Console.CursorLeft = pos.x;
                        Console.Write("                          ");
                        Console.CursorLeft = pos.x;
                        anim.Reset();
                        hasSizes = true;
                    }
                    #endregion

                    bool dirSelected = selected < directories.Count + 1;
                    int fileIndex = selected - directories.Count - 1;

                    #region printDirsAndFiles
                    Console.CursorTop += 1;

                    for (int i = 0; i < directories.Count; i++) // print directories
                    {
                        Console.CursorLeft = pos.x;
                        (string name, double size, string time) cDir = directories[i];

                        double size = MathPlus.Round(ChooseAppropriate(cDir.size, Unit.B, out Unit unit), 2);

                        Console.CursorLeft = pos.x;
                        if (dirSelected && i == selected - 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.WriteLine($"{cDir.time}{new string(' ', dirLongestTime - cDir.time.Length)}" +
                                              $"{cDir.name}{new string(' ', dirLongestName - cDir.name.Length)}" +
                                              $"{size} {unit}");
                            Console.ResetColor();
                        }
                        else
                            Console.WriteLine($"{cDir.time}{new string(' ', dirLongestTime - cDir.time.Length)}" +
                                              $"{cDir.name}{new string(' ', dirLongestName - cDir.name.Length)}" +
                                              $"{size} {unit}");
                    }

                    Console.CursorTop += 2;

                    for (int i = 0; i < files.Count; i++) // print files
                    {
                        Console.CursorLeft = pos.x;
                        (string name, double size, string time) cFile = files[i];

                        double size = MathPlus.Round(ChooseAppropriate(cFile.size, Unit.B, out Unit unit), 2);

                        Console.CursorLeft = pos.x;
                        if (!dirSelected && i == fileIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.WriteLine($"{cFile.time}{new string(' ', fileLongestTime - cFile.time.Length)}" +
                                          $"{cFile.name}{new string(' ', fileLongestName - cFile.name.Length)}" +
                                          $"{size} {unit}");
                            Console.ResetColor();
                        }
                        else
                            Console.WriteLine($"{cFile.time}{new string(' ', fileLongestTime - cFile.time.Length)}" +
                                          $"{cFile.name}{new string(' ', fileLongestName - cFile.name.Length)}" +
                                          $"{size} {unit}");
                    }
                    #endregion



                    Console.CursorTop++;
                    Console.CursorLeft = pos.x;
                    Console.Write("Selected: " + Selected + new string(' ', Console.WindowWidth));
                    Console.CursorLeft = pos.x;
                    ConsoleExtensions.Write("Cancel", selected == directories.Count + files.Count + 1);
                    Console.CursorTop++;
                    Console.CursorLeft = pos.x;
                    if (Selected != "")
                        ConsoleExtensions.Write("Ok", selected == directories.Count + files.Count + 2);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("Ok");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    int scroolY = pos.y + selected + 5 + (dirSelected ? 0 : 2) + (selected == 0 ? -1 : 0) +
                        (selected > directories.Count + files.Count ? 1 : 0) - 5;
                    if (scroolY < 0)
                        scroolY = 0;

                    Console.SetCursorPosition(0, scroolY);

                    #region handleInput
                    Arrows arrow = GetArrow();

                    if (arrow == Arrows.Up && selected > 0)
                        selected--;
                    else if (arrow == Arrows.Down && selected < directories.Count + files.Count + 2)
                        selected++;
                    else if (arrow == Arrows.Enter)
                    {
                        if (selected == 0)
                        {
                            if (path.Length == 3)
                            {
                                path = "This PC:";
                                selected = 0;
                                hasSizes = false;
                            }
                            else
                            {
                                while (path.Length > 0 && (path.Last() == '\\' || path.Last() == '/'))
                                    path = path.Substring(0, path.Length - 1);
                                path = Directory.GetParent(path).FullName;
                                selected = 0;
                                hasSizes = false;
                            }
                            Console.Clear();
                        }
                        else if (dirSelected)
                        {
                            if (path.Last() != '\\' && path.Last() != '/')
                                path += '\\';
                            path += directories[selected - 1].name;
                            selected = 0;
                            hasSizes = false;
                            Console.Clear();
                        }
                        else if (selected < directories.Count + files.Count + 1)
                        {
                            Selected = path + files[fileIndex].name;
                        }
                        else
                        {
                            if (selected == directories.Count + files.Count + 1)
                                return STATUS.CANCEL;
                            else if (selected == directories.Count + files.Count + 2 && Selected != "")
                                return STATUS.OK;
                        }
                    }
                    #endregion
                } // not drive end
            } // while end
        }

        public STATUS Show(Vector2Int pos, Image image)
        {
            throw new NotImplementedException();
        }

        public STATUS Show(Vector2Int pos, ConsoleImage image)
        {
            throw new NotImplementedException();
        }

        public void Render(Vector2Int pos)
        {
            throw new NotImplementedException();
        }

        public void Render(Vector2Int pos, Image image)
        {
            throw new NotImplementedException();
        }

        public void Render(Vector2Int pos, ConsoleImage image)
        {
            throw new NotImplementedException();
        }

        public void SetSelected(int _selected)
        {
            throw new NotImplementedException();
        }
    }
}
