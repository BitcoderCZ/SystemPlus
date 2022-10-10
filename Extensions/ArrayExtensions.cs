using System;
using System.Collections.Generic;
using System.Linq;
using SystemPlus.ClassSupport;
using SystemPlus.Vectors;

namespace SystemPlus.Extensions
{
    public static class ArrayExtensions
    {
        public static bool ContainsChar(this char[,] array, char ch, out int count)
        {
            int c = 0;
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    if (array[x, y] == ch)
                        c++;

            count = c;
            return c > 0;
        }

        public static bool InBounds<T>(this T[] array, int index)
        {
            if (index >= 0 && index < array.Length)
                return true;
            else
                return false;
        }

        public static bool InBounds<T>(this T[] array, Vector2Int pos, int width)
        {
            if (pos.x < 0 || pos.y < 0)
                return false;
            else if (pos.x >= width || pos.y >= array.GetHeight(width))
                return false;
            else
                return true;
        }

        public static int GetHeight<T>(this T[] array, int width)
            => (int)Math.Ceiling((double)array.Length / (double)width);

        public static int GetX<T>(this T[] array, int i, int width)
            => i % (width - 1);

        public static int GetX(int i, int width)
            => i % (width - 1);

        public static T[] FindAll<T>(this T[] array, Func<T, bool> selector)
        {
            List<T> ts = new List<T>();
            for (int i = 0; i < array.Length; i++)
                if (selector.Invoke(array[i]))
                    ts.Add(array[i]);

            return ts.ToArray();
        }

        public static T[] Clone<T>(this T[] array) where T : ICloneSupport<T>
        {
            T[] clone = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
                clone[i] = array[i].Clone();

            return clone;
        }

        public static List<T> Clone<T>(this List<T> list) where T : ICloneSupport<T>
        {
            T[] clone = new T[list.Count];

            for (int i = 0; i < list.Count; i++)
                clone[i] = list[i].Clone();

            return clone.ToList();
        }

        public static T2[] ToNewType<T1, T2>(this T1[] array, Func<T1, T2> func)
        {
            T2[] newArray = new T2[array.Length];
            for (int i = 0; i < array.Length; i++)
                newArray[i] = func.Invoke(array[i]);

            return newArray;
        }

        public static T2[] ChangeType<T1, T2>(this T1[] array, Func<T1, T2> func)
            => array.ToNewType(func);

        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                    action.Invoke(array[i]);
        }

        public static T Find<T>(this T[] array, Func<T, bool> func)
        {
            foreach (T t in array)
                if (func(t))
                    return t;

            return default(T);
        }

        public static T[] Split<T>(this T[] array, int index, int length)
        {
            T[] newAR = new T[length];
            for (int i = index; i < index + length; i++)
                newAR[i - index] = array[i];
            return newAR;
        }

        public static void StandardNormals(this double[] array, System.Random random, double devider)
        {
            for (int x = 0; x < array.Length; x++)
            {
                bool assinged = false;
                int i = 0;
                while (i < 50)
                {
                    int iNearZero = random.Next(0, 3);
                    bool nearZero = (iNearZero > 0);
                    int iValue = random.Next(-300000000, 300000000);
                    double value = (double)iValue / 100000000d;
                    if (!nearZero)
                    {
                        array[x] = value / (double)devider;
                        assinged = true;
                        break;
                    }
                    else
                    {
                        if (value < -1d || value > 1d)
                            continue;
                        else
                        {
                            array[x] = value / (double)devider;
                            assinged = true;
                            break;
                        }
                    }
                }
                if (!assinged)
                {
                    int iValue = random.Next(-3000, 3000);
                    array[x] = ((double)iValue / 1000d) / (double)devider;
                }
            }
        }

        public static void Print<T>(this T[] array)
        {
            Console.Write(" [");
            for (int i = 0; i < array.Length; i++)
            {
                if (i != array.Length - 1)
                    Console.Write(array[i] + ", ");
                else
                    Console.Write(array[i]);
            }
            Console.Write("]\n");
        }

        public static bool InBounds<T>(this T[,] array, Vector2Int index)
        {
            if (index.x >= 0 && index.x < array.GetLength(1) && index.y >= 0 && index.y < array.GetLength(0))
                return true;
            else
                return false;
        }

        public static bool InBounds<T>(this T[,] array, int x, int y)
        {
            if (x >= 0 && x < array.GetLength(1) && y >= 0 && y < array.GetLength(0))
                return true;
            else
                return false;
        }

        public static bool Contains<T>(this T[,] array, T item, out int times)
        {
            bool contains = false;
            times = 0;
            for (int x = 0; x < array.GetLength(1); x++)
                for (int y = 0; y < array.GetLength(0); y++)
                    if (Equals(array[y, x], item))
                    {
                        contains = true;
                        times++;
                    }

            return contains;
        }

        public static Vector2Int IndexOf<T>(this T[,] array, T idexof)
        {
            for (int x = 0; x < array.GetLength(1); x++)
                for (int y = 0; y < array.GetLength(0); y++)
                    if (Equals(array[y, x], idexof))
                        return new Vector2Int(x, y);

            return null;
        }

        public static T ElementAt<T>(this T[,] array, Vector2Int index)
        {
            if (!array.InBounds(index))
                return default(T);
            else
                return array[index.y, index.x];
        }

        public static T ElementAt<T>(this T[,] array, int x, int y)
        {
            if (!array.InBounds(x, y))
                return default(T);
            else
                return array[y, x];
        }

        public static void Foreach<T>(this T[,] array, Action<T> action)
        {
            for (int x = 0; x < array.GetLength(1); x++)
                for (int y = 0; y < array.GetLength(0); y++)
                    if (array[y, x] != null)
                        action.Invoke(array[y, x]);
        }

        public static void Fill<T>(this T[] array, T value)
        {
            var length = array.Length;
            if (length == 0) return;

            int seed = Math.Min(32, array.Length);
            for (var i = 0; i < seed; i++)
                array[i] = value;

            int count;
            for (count = seed; count <= length / 2; count *= 2)
                Array.Copy(array, 0, array, count, count);

            int leftover = length - count;
            if (leftover > 0)
                Array.Copy(array, 0, array, count, leftover);
        }

        public static void StandardNormals(this double[,] array, System.Random random, double devider)
        {
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    bool assinged = false;
                    int i = 0;
                    while (i < 50)
                    {
                        int iNearZero = random.Next(0, 3);
                        bool nearZero = (iNearZero > 0);
                        int iValue = random.Next(-300000000, 300000000);
                        double value = (double)iValue / 100000000d;
                        if (!nearZero)
                        {
                            array[x, y] = value / (double)devider;
                            assinged = true;
                            break;
                        }
                        else
                        {
                            if (value < -1d || value > 1d)
                                continue;
                            else
                            {
                                array[x, y] = value / (double)devider;
                                assinged = true;
                                break;
                            }
                        }
                    }
                    if (!assinged)
                    {
                        int iValue = random.Next(-3000, 3000);
                        array[x, y] = ((double)iValue / 1000d) / (double)devider;
                    }
                }
        }

        public static void Print<T>(this T[][][] array)
        {
            for (int x = 0; x < array.Length; x++)
            {
                Console.Write(" [");
                for (int y = 0; y < array[x].Length; y++)
                {
                    if (y != 0)
                        Console.Write("   [");
                    else
                        Console.Write(" [");
                    for (int z = 0; z < array[x][y].Length; z++)
                    {
                        if (z != array[x][y].Length - 1)
                            Console.Write(array[x][y][z] + ", ");
                        else
                            Console.Write(array[x][y][z]);
                    }
                    if (y != array[x].Length - 1)
                        Console.Write("]\n");
                    else
                        Console.Write("]");
                }
                Console.Write(" ]\n\n");
            }
        }

        public static void StandardNormals3D(System.Random random, double devider, ref double[][][] array)
        {
            for (int x = 0; x < array.Length; x++)
                for (int y = 0; y < array[x].Length; y++)
                    for (int z = 0; z < array[x][y].Length; z++)
                    {
                        bool assinged = false;
                        int i = 0;
                        while (i < 50)
                        {
                            int iNearZero = random.Next(0, 3);
                            bool nearZero = (iNearZero > 0);
                            int iValue = random.Next(-300000000, 300000000);
                            double value = (double)iValue / 100000000d;
                            if (!nearZero)
                            {
                                array[x][y][z] = value / (double)devider;
                                assinged = true;
                                break;
                            }
                            else
                            {
                                if (value < -1d || value > 1d)
                                    continue;
                                else
                                {
                                    array[x][y][z] = value / (double)devider;
                                    assinged = true;
                                    break;
                                }
                            }
                        }
                        if (!assinged)
                        {
                            int iValue = random.Next(-3000, 3000);
                            array[x][y][z] = ((double)iValue / 1000d) / (double)devider;
                        }
                    }
        }

        public static T[][] Create2D<T>(int x, int y)
        {
            T[][] ar = new T[x][];
            for (int i = 0; i < x; i++)
                ar[x] = new T[y];
            return ar;
        }
    }
}
