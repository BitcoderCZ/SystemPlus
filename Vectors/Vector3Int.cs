using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector3Int : ICloneSupport<Vector3Int>
    {
        public int x;
        public int y;
        public int z;

        #region ctors
        public Vector3Int()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3Int(int _x, int _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }

        public Vector3Int(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vector3Int(string _x, string _y, string _z)
        {
            x = int.Parse(_x);
            y = int.Parse(_y);
            z = int.Parse(_z);
        }
        #endregion

        #region operations
        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3Int operator *(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3Int operator /(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3Int operator /(Vector3Int a, int b)
        {
            return new Vector3Int(a.x / b, a.y / b, a.z / b);
        }
        public static bool operator ==(Vector3Int a, Vector3Int b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3Int a, Vector3Int b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
        public static bool operator >(Vector3Int a, Vector3Int b)
        {
            return a.x > b.x && a.y > b.y && a.z > b.z;
        }
        public static bool operator <(Vector3Int a, Vector3Int b)
        {
            return a.x < b.x && a.y < b.y && a.z < b.z;
        }
        public static bool operator >=(Vector3Int a, Vector3Int b)
        {
            return a.x >= b.x && a.y >= b.y && a.z >= b.z;
        }
        public static bool operator <=(Vector3Int a, Vector3Int b)
        {
            return a.x <= b.x && a.y <= b.y && a.z <= b.z;
        }
        public static Vector3Int operator -(Vector3Int v) => new Vector3Int(-v.x, -v.y, -v.z);
        #endregion

        public static Vector3Int Zero => new Vector3Int(0, 0);

        public static Vector3Int Up => new Vector3Int(0, 1);
        public static Vector3Int Down => new Vector3Int(0, -1);
        public static Vector3Int Left => new Vector3Int(-1, 0);
        public static Vector3Int Right => new Vector3Int(1, 0);
        public static Vector3Int Forward => new Vector3Int(0, 0, 1);
        public static Vector3Int Back => new Vector3Int(0, 0, -1);

        public override string ToString() => $"x: {x} y: {y} z: {z}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector3Int))
                return false;

            return this == (Vector3Int)obj;
        }

        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }

        public Vector3Int Clone()
        {
            return new Vector3Int(x, y, z);
        }
    }
}
