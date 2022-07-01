using System;
using System.Collections.Generic;
using System.IO;

namespace SystemPlus.Extensions
{
    public static class IOExtensions
    {
        public class Unit
        {
            internal byte n;

            public static readonly Unit B = new Unit(1);//1024
            public static readonly Unit KB = new Unit(2);
            public static readonly Unit MB = new Unit(3);
            public static readonly Unit GB = new Unit(4);
            public static readonly Unit TB = new Unit(5);

            internal Unit(byte _s) { n = _s; }

            public static bool operator ==(Unit a, Unit b) => a.n == b.n;
            public static bool operator !=(Unit a, Unit b) => a.n != b.n;

            public override bool Equals(object obj)
            {
                if (obj is Unit u)
                    return u == this;
                else
                    return false;
            }

            public override int GetHashCode() => n;

            public override string ToString()
            {
                if (units.TryGetValue(n, out string s))
                    return s;
                else
                    return null;
            }

            static Dictionary<int, string> units = new Dictionary<int, string>()
            {
                { 1, "B" },
                { 2, "KB" },
                { 3, "MB" },
                { 4, "GB" },
                { 5, "TB" },
            };
        }

        public static double Convert(double size, Unit from, Unit to)
        {
            if (from == to)
                return size;

            if (to.n < from.n)
                for (byte i = from.n; i >= 1; i--)
                {
                    if (new Unit(i) == to)
                        return size;
                    else
                        size *= 1024d;
                }
            else
                for (byte i = from.n; i <= Unit.TB.n; i++)
                {
                    if (new Unit(i) == to)
                        return size;
                    else
                        size /= 1024d;
                }

            return double.NaN;
        }

        public static double ChooseAppropriate(double size, Unit originalUnit, out Unit newUnit)
        {
            if (size > 1024d && originalUnit != Unit.TB)
                return ChooseAppropriate(size / 1024d, new Unit((byte)(originalUnit.n + 1)), out newUnit);
            else if (size < 1d && originalUnit != Unit.B)
                return ChooseAppropriate(size * 1024d, new Unit((byte)(originalUnit.n - 1)), out newUnit);
            else
            {
                newUnit = originalUnit;
                return size;
            }
        }

        public static long Directory_GetSize(string path, bool saveSize, bool saveSubDirSizes, out string e)
        {
            e = null;
            if (File.Exists(path + "SIZE"))
            {
                File.SetAttributes(path + "SIZE", File.GetAttributes(path + "SIZE") | FileAttributes.Hidden);
                return long.Parse(File.ReadAllText(path + "SIZE"));
            }

            long size = 0;

            string[] files;

            //Console.WriteLine(path);

            try
            {
                files = Directory.GetFiles(path);

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo info = new FileInfo(files[i]);
                    size += info.Length;
                }

                string[] directories = Directory.GetDirectories(path);

                for (int i = 0; i < directories.Length; i++)
                    size += Directory_GetSize(directories[i], saveSubDirSizes, saveSubDirSizes, out string ee);
            }
            catch { return 0; }

            if (saveSize)
            {
                try
                {
                    StreamWriter writer = File.CreateText(path + "SIZE");
                    writer.Write(size);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    File.SetAttributes(path + "SIZE", File.GetAttributes(path + "SIZE") | FileAttributes.Hidden);
                }
                catch (Exception eee) { e = eee.ToString(); }
            }

            return size;
        }

        public static void Directory_Delete(string path)
        {
            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
                try
                {
                    File.Delete(files[i]);
                }
                catch { }

            string[] directories = Directory.GetDirectories(path);

            for (int i = 0; i < directories.Length; i++)
                Directory_Delete(directories[i]);

            try
            {
                Directory.Delete(path);
            }
            catch { }
        }
    }
}
