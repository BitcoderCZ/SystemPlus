using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector2Int : ICloneSupport<Vector2Int>
    {
        public int x;
        public int y;

        #region ctors
        public Vector2Int()
        {
            x = 0;
            y = 0;
        }

        public Vector2Int(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public Vector2Int(string _x, string _y)
        {
            x = int.Parse(_x);
            y = int.Parse(_y);
        }
        #endregion

        #region operations
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }
        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }
        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }
        public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x / b.x, a.y / b.y);
        }
        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }
        public static Vector2Int operator /(Vector2Int a, int b)
        {
            return new Vector2Int(a.x / b, a.y / b);
        }
        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public static bool operator >(Vector2Int a, Vector2Int b)
        {
            return a.x > b.x && a.y > b.y;
        }
        public static bool operator <(Vector2Int a, Vector2Int b)
        {
            return a.x < b.x && a.y < b.y;
        }
        public static bool operator >=(Vector2Int a, Vector2Int b)
        {
            return a.x >= b.x && a.y >= b.y;
        }
        public static bool operator <=(Vector2Int a, Vector2Int b)
        {
            return a.x <= b.x && a.y <= b.y;
        }
        public static Vector2Int operator -(Vector2Int v) => new Vector2Int(-v.x, -v.y);
        #endregion


        public static explicit operator Vector2(Vector2Int v) => new Vector2(v.x, v.y);


        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int One => new Vector2Int(1, 1);

        public static Vector2Int Up => new Vector2Int(0, 1);
        public static Vector2Int Down => new Vector2Int(0, -1);
        public static Vector2Int Left => new Vector2Int(-1, 0);
        public static Vector2Int Right => new Vector2Int(1, 0);

        public override string ToString() => $"X {x} Y {y}";

        public static Vector2Int FromString(string s)
        {
            int state = 0;
            int x = 0;
            int y = 0;

            for (int i = 0; i < s.Length; i++)
            {
                ArrayRef<char> c = new ArrayRef<char>(i, s.ToCharArray());
                if (state == 0 && c == 'X')
                {
                    state = 1;
                }
                else if (state == 1 && char.IsDigit(c))
                {
                    int stop = -1;
                    for (int j = i; j < s.Length; j++)
                    {
                        ArrayRef<char> c2 = new ArrayRef<char>(j, s.ToCharArray());
                        if (!char.IsDigit(c2))
                        {
                            stop = j;
                            break;
                        }
                    }

                    if (stop == -1)
                        return null;

                    x = int.Parse(s.Substring(i, stop - i));

                    state = 2;

                    i = stop - 1;
                }
                else if (state == 2 && c == 'Y')
                {
                    state = 3;
                }
                else if (state == 3 && char.IsDigit(c))
                {
                    int stop = -1;
                    for (int j = i; j < s.Length; j++)
                    {
                        if (!char.IsDigit(s[j]))
                        {
                            stop = j;
                            break;
                        }
                    }

                    if (stop == -1)
                        stop = s.Length;

                    y = int.Parse(s.Substring(i, stop - i));

                    break;
                }
            }

            return new Vector2Int(x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector2Int))
                return false;

            return this == (Vector2Int)obj;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public Vector2Int Clone()
        {
            return new Vector2Int(x, y);
        }
    }
}
