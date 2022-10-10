using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector3Short : ICloneSupport<Vector3Short>
    {
        public short x;
        public short y;
        public short z;

        #region ctors
        public Vector3Short()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3Short(short _x, short _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }

        public Vector3Short(short _x, short _y, short _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vector3Short(string _x, string _y, string _z)
        {
            x = short.Parse(_x);
            y = short.Parse(_y);
            z = short.Parse(_z);
        }
        #endregion

        #region operations
        public static Vector3Short operator +(Vector3Short a, Vector3Short b)
        {
            return new Vector3Short((short)(a.x + b.x), (short)(a.y + b.y), (short)(a.z + b.z));
        }
        public static Vector3Short operator -(Vector3Short a, Vector3Short b)
        {
            return new Vector3Short((short)(a.x - b.x), (short)(a.y - b.y), (short)(a.z - b.z));
        }
        public static Vector3Short operator *(Vector3Short a, Vector3Short b)
        {
            return new Vector3Short((short)(a.x * b.x), (short)(a.y * b.y), (short)(a.z * b.z));
        }
        public static Vector3Short operator /(Vector3Short a, Vector3Short b)
        {
            return new Vector3Short((short)(a.x / b.x), (short)(a.y / b.y), (short)(a.z / b.z));
        }
        public static Vector3Short operator *(Vector3Short a, short b)
        {
            return new Vector3Short((short)(a.x * b), (short)(a.y * b), (short)(a.z * b));
        }
        public static Vector3Short operator /(Vector3Short a, short b)
        {
            return new Vector3Short((short)(a.x / b), (short)(a.y / b), (short)(a.z / b));
        }
        public static bool operator ==(Vector3Short a, Vector3Short b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3Short a, Vector3Short b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
        public static bool operator >(Vector3Short a, Vector3Short b)
        {
            return a.x > b.x && a.y > b.y && a.z > b.z;
        }
        public static bool operator <(Vector3Short a, Vector3Short b)
        {
            return a.x < b.x && a.y < b.y && a.z < b.z;
        }
        public static bool operator >=(Vector3Short a, Vector3Short b)
        {
            return a.x >= b.x && a.y >= b.y && a.z >= b.z;
        }
        public static bool operator <=(Vector3Short a, Vector3Short b)
        {
            return a.x <= b.x && a.y <= b.y && a.z <= b.z;
        }
        public static Vector3Short operator -(Vector3Short v) => new Vector3Short((short)-v.x, (short)-v.y, (short)-v.z);
        #endregion

        public static Vector3Short Zero => new Vector3Short(0, 0);

        public static Vector3Short Up => new Vector3Short(0, 1);
        public static Vector3Short Down => new Vector3Short(0, -1);
        public static Vector3Short Left => new Vector3Short(-1, 0);
        public static Vector3Short Right => new Vector3Short(1, 0);
        public static Vector3Short Forward => new Vector3Short(0, 0, 1);
        public static Vector3Short Back => new Vector3Short(0, 0, -1);

        public override string ToString() => $"x: {x} y: {y} z: {z}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector3Short))
                return false;

            return this == (Vector3Short)obj;
        }

        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }

        public Vector3Short Clone()
        {
            return new Vector3Short(x, y, z);
        }
    }
}
