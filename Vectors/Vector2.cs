using System;
using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector2 : ICloneSupport<Vector2>
    {
        public float x;
        public float y;

        #region ctors
        public Vector2()
        {
            x = 0f;
            y = 0f;
        }

        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public Vector2(string _x, string _y)
        {
            x = float.Parse(_x.Replace('.', ','));
            y = float.Parse(_y.Replace('.', ','));
        }
        #endregion

        #region operations
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public static bool operator >(Vector2 a, Vector2 b)
        {
            return a.x > b.x && a.y > b.y;
        }
        public static bool operator <(Vector2 a, Vector2 b)
        {
            return a.x < b.x && a.y < b.y;
        }
        public static bool operator >=(Vector2 a, Vector2 b)
        {
            return a.x >= b.x && a.y >= b.y;
        }
        public static bool operator <=(Vector2 a, Vector2 b)
        {
            return a.x <= b.x && a.y <= b.y;
        }
        public static Vector2 operator -(Vector2 v) => new Vector2(-v.x, -v.y);
        #endregion

        public static Vector2 Zero => new Vector2(0f, 0f);

        public static Vector2 Up => new Vector2(0f, 1f);
        public static Vector2 Down => new Vector2(0f, -1f);
        public static Vector2 Left => new Vector2(-1f, 0f);
        public static Vector2 Right => new Vector2(1f, 0f);

        public static float DotProduct(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;
        public float Lenght() => (float)Math.Sqrt(DotProduct(this, this));
        public Vector2 Normalize() => new Vector2(x / Lenght(), y / Lenght());

        public static implicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.x, v.y);
        }
        public static implicit operator Vector4(Vector2 v)
        {
            return new Vector4(v.x, v.y);
        }

        public override string ToString() => $"x: {x} y: {y}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector2))
                return false;

            return this == (Vector2)obj;
        }

        public override int GetHashCode()
        {
            return int.Parse(x.ToString()) ^ int.Parse(y.ToString());
        }

        public Vector2 Clone()
        {
            return new Vector2(x, y);
        }
    }
}
