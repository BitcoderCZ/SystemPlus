using System;
using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public sealed class Vector3 : CloneSupport<Vector3>
    {
        public float x;
        public float y;
        public float z;

        #region ctors
        public Vector3()
        {
            x = 0f;
            y = 0f;
            z = 0f;
        }

        public Vector3(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0f;
        }

        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vector3(string _x, string _y, string _z)
        {
            x = float.Parse(_x);//.Replace('.', ','));
            y = float.Parse(_y);//.Replace('.', ','));
            z = float.Parse(_z);//.Replace('.', ','));
        }
        #endregion

        #region operations
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator +(Vector3 a, Vector4 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector4 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator *(Vector3 a, Vector4 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector4 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator +(Vector3 a, float b)
        {
            return new Vector3(a.x + b, a.y + b, a.z + b);
        }
        public static Vector3 operator -(Vector3 a, float b)
        {
            return new Vector3(a.x - b, a.y - b, a.z - b);
        }
        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3 operator /(Vector3 a, float b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.x != b.x || a.y != b.y && a.z != b.z;
        }
        public static bool operator >(Vector3 a, Vector3 b)
        {
            return a.x > b.x && a.y > b.y && a.z > b.z;
        }
        public static bool operator <(Vector3 a, Vector3 b)
        {
            return a.x < b.x && a.y < b.y && a.z < b.z;
        }
        public static bool operator >=(Vector3 a, Vector3 b)
        {
            return a.x >= b.x && a.y >= b.y && a.z >= b.z;
        }
        public static bool operator <=(Vector3 a, Vector3 b)
        {
            return a.x <= b.x && a.y <= b.y && a.z <= b.z;
        }
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.x, -v.y, -v.z);
        #endregion

        public static Vector3 Zero => new Vector3(0f, 0f, 0f);
        public static Vector3 UnitX
        {
            get { return new Vector3(1.0f, 0.0f, 0.0f); }
        }
        public static Vector3 UnitY
        {
            get { return new Vector3(0.0f, 1.0f, 0.0f); }
        }
        public static Vector3 UnitZ
        {
            get { return new Vector3(0.0f, 0.0f, 1.0f); }
        }
        public static Vector3 One
        {
            get { return new Vector3(1.0f, 1.0f, 1.0f); }
        }

        public static Vector3 Up => new Vector3(0f, 1f);
        public static Vector3 Down => new Vector3(0f, -1f);
        public static Vector3 Left => new Vector3(-1f, 0f);
        public static Vector3 Right => new Vector3(1f, 0f);
        public static Vector3 Forward => new Vector3(0f, 0f, 1f);
        public static Vector3 Back => new Vector3(0f, 0f, -1f);

        #region funcs
        public static float DotProduct(Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;
        public static Vector3 CrossProduct(Vector3 a, Vector3 b) =>
            new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        public float Length() => (float)Math.Sqrt(DotProduct(this, this));
        public Vector3 Normalize() => new Vector3(x / Length(), y / Length(), z / Length());
        public Vector3 Truncate()
        {
            float _x = (Math.Abs(x) - 0.0001f < 0) ? 0 : x;
            float _y = (Math.Abs(y) - 0.0001f < 0) ? 0 : y;
            float _z = (Math.Abs(z) - 0.0001f < 0) ? 0 : z;
            return new Vector3(_x, _y, _z);
        }
        public float Max() => (x >= y && x >= z) ? x : (y >= z) ? y : z;
        public float Min() => (x <= y && x <= z) ? x : (y <= z) ? y : z;
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float amount) => v1 + (v2 - v1) * amount;
        public static float CalculateAngle(Vector3 first, Vector3 second) =>
            (float)Math.Acos((DotProduct(first, second)) / (first.Length() * second.Length()));
        public static Vector3 Normalize(Vector3 v)
        {
            if (v.Length() == 0) return Zero;
            else return new Vector3(v.x, v.y, v.z) / v.Length();
        }
        public Quaternion GetRotationTo(Vector3 destination)
        {
            // Based on Stan Melax's algorithm in "Game Programming Gems"
            Vector3 t_source = this.Normalize();
            Vector3 t_dest = destination.Normalize();

            float d = DotProduct(t_source, t_dest);// t_source.Dot(t_dest);

            // if dot == 1 then the vectors are the same
            if (d >= 1.0f) return Quaternion.Identity;
            else if (d < (1e-6f - 1.0f))
            {
                Vector3 t_axis = CrossProduct(UnitX, this);
                if (Math.Sqrt(t_axis.Length()) < (1e-12)) // pick another if colinear
                    t_axis = CrossProduct(UnitY, this);
                t_axis.Normalize();
                return Quaternion.FromAngleAxis((float)Math.PI, t_axis);
            }
            else
            {
                float t_sqrt = (float)Math.Sqrt((1 + d) * 2.0f);
                float t_invs = 1.0f / t_sqrt;

                Vector3 t_cross = CrossProduct(t_source, t_dest); //t_source.Cross(t_dest);
                return new Quaternion(t_cross.x * t_invs, t_cross.y * t_invs, t_cross.z * t_invs, t_sqrt * 0.5f).Normalize();
            }
        }
        public static Vector3 Abs(Vector3 value) =>
            new Vector3(Math.Abs(value.x), Math.Abs(value.y), Math.Abs(value.z));
        public static Vector3 Translate(Vector3 original, Vector3 translation)
        {
            Vector3 toReturn = new Vector3();
            toReturn.x = original.x + translation.x;
            toReturn.y = original.y + translation.y;
            toReturn.z = original.z + translation.z;
            return toReturn;
        }
        public static Vector3 Rotate(Vector3 original, Vector3 rotation)
        {
            Vector3 toReturn = new Vector3();
            // Rotation matrix: https://en.wikipedia.org/wiki/Rotation_matrix
            toReturn.x = original.x * ((float)Math.Cos(rotation.z) * (float)Math.Cos(rotation.y)) +
                         original.y * ((float)Math.Cos(rotation.z) * (float)Math.Sin(rotation.y) * (float)Math.Sin(rotation.x) -
                         (float)Math.Sin(rotation.z) * (float)Math.Cos(rotation.x)) +
                         original.z * ((float)Math.Cos(rotation.z) * (float)Math.Sin(rotation.y) * (float)Math.Cos(rotation.x) +
                         (float)Math.Sin(rotation.z) * (float)Math.Sin(rotation.x));
            toReturn.y = original.x * ((float)Math.Sin(rotation.z) * (float)Math.Cos(rotation.y)) +
                         original.y * ((float)Math.Sin(rotation.z) * (float)Math.Sin(rotation.y) * (float)Math.Sin(rotation.x) +
                         (float)Math.Cos(rotation.z) * (float)Math.Cos(rotation.x)) +
                         original.z * ((float)Math.Sin(rotation.z) * (float)Math.Sin(rotation.y) * (float)Math.Cos(rotation.x) -
                         (float)Math.Cos(rotation.z) * (float)Math.Sin(rotation.x));
            toReturn.z = original.x * (-(float)Math.Sin(rotation.y)) +
                         original.y * ((float)Math.Cos(rotation.y) * (float)Math.Sin(rotation.x)) +
                         original.z * ((float)Math.Cos(rotation.y) * (float)Math.Cos(rotation.x));
            return toReturn;
        }
        public static Vector3 ApplyPerspective(Vector3 original, float Z0)
        {
            Vector3 toReturn = new Vector3();
            toReturn.x = original.x * Z0 / (Z0 + original.z);
            toReturn.y = original.y * Z0 / (Z0 + original.z);
            toReturn.z = original.z;

            /*toReturn.u = original.u * Z0 / (Z0 + original.z);
            toReturn.v = original.v * Z0 / (Z0 + original.z);
            toReturn.w = original.w * Z0 / (Z0 + original.z);*/

            return toReturn;
        }
        public static Vector3 CenterScreen(Vector3 original, Vector3 Size)
        {
            Vector3 toReturn = new Vector3();
            toReturn.x = original.x + Size.x / 2f;
            toReturn.y = original.y + Size.y / 2f;
            toReturn.z = original.z;
            /*toReturn.u = original.u;
            toReturn.v = original.v;
            toReturn.w = original.w;*/
            return toReturn;
        }
        #endregion

        public static implicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
        public static implicit operator Vector4(Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z);
        }

        public override string ToString() => $"x: {x} y: {y} z: {z}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector3))
                return false;

            return this == (Vector3)obj;
        }

        public override int GetHashCode()
        {
            return int.Parse(x.ToString()) ^ int.Parse(y.ToString()) ^ int.Parse(z.ToString());
        }

        public override Vector3 Clone()
        {
            return new Vector3(x, y, z);
        }
    }

    public static class Vector3Extensions
    {
        public static void TakeMin(this Vector3 tv, Vector3 v) // was this ref Ve...
        {
            if (v.x < tv.x) tv.x = v.x;
            if (v.y < tv.y) tv.y = v.y;
            if (v.z < tv.z) tv.z = v.z;
        }
        public static void TakeMax(this Vector3 tv, Vector3 v) // was this ref Ve...
        {
            if (v.x > tv.x) tv.x = v.x;
            if (v.y > tv.y) tv.y = v.y;
            if (v.z > tv.z) tv.z = v.z;
        }
    }
}
