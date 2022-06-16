using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector2Short : CloneSupport<Vector2Short>
    {
        public short x;
        public short y;

        #region ctors
        public Vector2Short()
        {
            x = 0;
            y = 0;
        }

        public Vector2Short(short _x, short _y)
        {
            x = _x;
            y = _y;
        }
        public Vector2Short(string _x, string _y)
        {
            x = short.Parse(_x);
            y = short.Parse(_y);
        }
        #endregion

        #region operations
        public static Vector2Short operator +(Vector2Short a, Vector2Short b)
        {
            return new Vector2Short((short)(a.x + b.x), (short)(a.y + b.y));
        }
        public static Vector2Short operator -(Vector2Short a, Vector2Short b)
        {
            return new Vector2Short((short)(a.x - b.x), (short)(a.y - b.y));
        }
        public static Vector2Short operator *(Vector2Short a, Vector2Short b)
        {
            return new Vector2Short((short)(a.x * b.x), (short)(a.y * b.y));
        }
        public static Vector2Short operator /(Vector2Short a, Vector2Short b)
        {
            return new Vector2Short((short)(a.x / b.x), (short)(a.y / b.y));
        }
        public static Vector2Short operator *(Vector2Short a, short b)
        {
            return new Vector2Short((short)(a.x * b), (short)(a.y * b));
        }
        public static Vector2Short operator /(Vector2Short a, short b)
        {
            return new Vector2Short((short)(a.x / b), (short)(a.y / b));
        }
        public static bool operator ==(Vector2Short a, Vector2Short b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2Short a, Vector2Short b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public static bool operator >(Vector2Short a, Vector2Short b)
        {
            return a.x > b.x && a.y > b.y;
        }
        public static bool operator <(Vector2Short a, Vector2Short b)
        {
            return a.x < b.x && a.y < b.y;
        }
        public static bool operator >=(Vector2Short a, Vector2Short b)
        {
            return a.x >= b.x && a.y >= b.y;
        }
        public static bool operator <=(Vector2Short a, Vector2Short b)
        {
            return a.x <= b.x && a.y <= b.y;
        }
        public static Vector2Short operator -(Vector2Short v) => new Vector2Short((short)-v.x, (short)-v.y);
        #endregion


        public static explicit operator Vector2(Vector2Short v) => new Vector2(v.x, v.y);
        public static implicit operator Vector2Short(Vector2 v) => new Vector2Short((short)MathPlus.Round(v.x), (short)MathPlus.Round(v.y));

        public static implicit operator Vector2Short(Vector3 v) => new Vector2(v.x, v.y);
        public static implicit operator Vector2Short(Vector3Int v) => new Vector2(v.x, v.y);
        public static implicit operator Vector2Short(Vector3Short v) => new Vector2(v.x, v.y);
        public static implicit operator Vector2Short(Vector4 v) => new Vector2(v.x, v.y);

        public static Vector2Short Zero => new Vector2Short(0, 0);

        public static Vector2Short Up => new Vector2Short(0, 1);
        public static Vector2Short Down => new Vector2Short(0, -1);
        public static Vector2Short Left => new Vector2Short(-1, 0);
        public static Vector2Short Right => new Vector2Short(1, 0);

        public override string ToString() => $"x: {x} y: {y}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector2Short))
                return false;

            return this == (Vector2Short)obj;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override Vector2Short Clone()
        {
            return new Vector2Short(x, y);
        }
    }
}
