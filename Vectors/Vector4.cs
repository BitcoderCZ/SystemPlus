using System;
using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector4 : CloneSupport<Vector4>
    {
        public float x;
        public float y;
        public float z;
        public float w;

        #region ctors
        public Vector4()
        {
            x = 0f;
            y = 0f;
            z = 0f;
            w = 0f;
        }

        public Vector4(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0f;
            w = 0f;
        }

        public Vector4(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
            w = 0f;
        }

        public Vector4(string _x, string _y, string _z)
        {
            x = float.Parse(_x.Replace('.', ','));
            y = float.Parse(_y.Replace('.', ','));
            z = float.Parse(_z.Replace('.', ','));
            w = 0f;
        }

        public Vector4(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public Vector4(string _x, string _y, string _z, string _w)
        {
            x = float.Parse(_x.Replace('.', ','));
            y = float.Parse(_y.Replace('.', ','));
            z = float.Parse(_z.Replace('.', ','));
            w = float.Parse(_w.Replace('.', ','));
        }
        public Vector4(Vector2 v, float _z, float _w)
        {
            x = v.x;
            y = v.y;
            z = _z;
            w = _w;
        }
        public Vector4(Vector3 v, float _w)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = _w;
        }
        #endregion

        #region operations
        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }
        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }
        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }
        public static Vector4 operator /(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x / b.x, a.y / b.y, a.z / b.z, a.w * b.w);
        }
        public static Vector4 operator *(Vector4 a, float b)
        {
            return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
        }
        public static Vector4 operator /(Vector4 a, float b)
        {
            return new Vector4(a.x / b, a.y / b, a.z / b, a.w / b);
        }
        public static Vector4 operator *(float b, Vector4 a)
        {
            return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
        }
        public static Vector4 operator /(float b, Vector4 a)
        {
            return new Vector4(a.x / b, a.y / b, a.z / b, a.w / b);
        }
        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }
        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }
        public static bool operator >(Vector4 a, Vector4 b)
        {
            return a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;
        }
        public static bool operator <(Vector4 a, Vector4 b)
        {
            return a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;
        }
        public static bool operator >=(Vector4 a, Vector4 b)
        {
            return a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;
        }
        public static bool operator <=(Vector4 a, Vector4 b)
        {
            return a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;
        }
        public static Vector4 operator -(Vector4 v) => new Vector4(-v.x, -v.y, -v.z, -v.w);
        #endregion

        public static Vector4 Zero => new Vector4(0f, 0f);
        public static Vector4 UnitX
        {
            get { return new Vector4(1.0f, 0.0f, 0.0f, 0.0f); }
        }
        public static Vector4 UnitY
        {
            get { return new Vector4(0.0f, 1.0f, 0.0f, 0.0f); }
        }
        public static Vector4 UnitZ
        {
            get { return new Vector4(0.0f, 0.0f, 1.0f, 0.0f); }
        }
        public static Vector4 UnitW
        {
            get { return new Vector4(0.0f, 0.0f, 0.0f, 1.0f); }
        }
        public static Vector4 One
        {
            get { return new Vector4(1.0f, 1.0f, 1.0f, 1.0f); }
        }

        public static Vector4 Up => new Vector4(0f, 1f);
        public static Vector4 Down => new Vector4(0f, -1f);
        public static Vector4 Left => new Vector4(-1f, 0f);
        public static Vector4 Right => new Vector4(1f, 0f);
        public static Vector4 Forward => new Vector4(0f, 0f, 1f);
        public static Vector4 Back => new Vector4(0f, 0f, -1f);

        public static float DotProduct(Vector4 a, Vector4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        public float Lenght() => (float)Math.Sqrt(DotProduct(this, this));
        public Vector4 Normalize() => new Vector4(x / Lenght(), y / Lenght(), z / Lenght(), w / Lenght());
        public void CopyTo(float[] array, int offset)
        {
            array[offset] = x;
            array[offset + 1] = y;
            array[offset + 2] = z;
            array[offset + 3] = w;
        }

        /// <summary>
        /// Provide an accessor for each of the elements of the Vector structure.
        /// </summary>
        /// <param name="index">The element to access (0 = X, 1 = Y, 2 = Z, 3 = W).</param>
        public float Get(int index)
        {
            switch (index)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                case 3: return w;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static implicit operator Vector2(Vector4 v)
        {
            return new Vector2(v.x, v.y);
        }
        public static implicit operator Vector3(Vector4 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public override string ToString() => $"x: {x} y: {y} z: {z} w: {w}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector4))
                return false;

            return this == (Vector4)obj;
        }

        public override int GetHashCode()
        {
            return int.Parse(x.ToString()) ^ int.Parse(y.ToString()) ^ int.Parse(z.ToString()) ^ int.Parse(w.ToString());
        }

        public override Vector4 Clone()
        {
            return new Vector4(x, y, z, w);
        }
    }
}
