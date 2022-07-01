using SystemPlus.ClassSupport;

namespace SystemPlus.Vectors
{
    public class SizeI : CloneSupport<SizeI>
    {
        public int Width;
        public int Heigth;

        #region ctors
        public SizeI()
        {
            Width = 0;
            Heigth = 0;
        }

        public SizeI(int _width, int _heigth)
        {
            Width = _width;
            Heigth = _heigth;
        }
        public SizeI(string _width, string _heigth)
        {
            Width = int.Parse(_width);
            Heigth = int.Parse(_heigth);
        }
        #endregion

        #region operations
        public static SizeI operator +(SizeI a, SizeI b)
        {
            return new SizeI(a.Width + b.Width, a.Heigth + b.Heigth);
        }
        public static SizeI operator -(SizeI a, SizeI b)
        {
            return new SizeI(a.Width - b.Width, a.Heigth - b.Heigth);
        }
        public static SizeI operator *(SizeI a, SizeI b)
        {
            return new SizeI(a.Width * b.Width, a.Heigth * b.Heigth);
        }
        public static SizeI operator /(SizeI a, SizeI b)
        {
            return new SizeI(a.Width / b.Width, a.Heigth / b.Heigth);
        }
        public static SizeI operator *(SizeI a, int b)
        {
            return new SizeI(a.Width * b, a.Heigth * b);
        }
        public static SizeI operator /(SizeI a, int b)
        {
            return new SizeI(a.Width / b, a.Heigth / b);
        }
        public static bool operator ==(SizeI a, SizeI b)
        {
            return a.Width == b.Width && a.Heigth == b.Heigth;
        }
        public static bool operator !=(SizeI a, SizeI b)
        {
            return a.Width != b.Width || a.Heigth != b.Heigth;
        }
        public static bool operator >(SizeI a, SizeI b)
        {
            return a.Width > b.Width && a.Heigth > b.Heigth;
        }
        public static bool operator <(SizeI a, SizeI b)
        {
            return a.Width < b.Width && a.Heigth < b.Heigth;
        }
        public static bool operator >=(SizeI a, SizeI b)
        {
            return a.Width >= b.Width && a.Heigth >= b.Heigth;
        }
        public static bool operator <=(SizeI a, SizeI b)
        {
            return a.Width <= b.Width && a.Heigth <= b.Heigth;
        }
        public static SizeI operator -(SizeI v) => new SizeI(-v.Width, -v.Heigth);
        #endregion


        public static SizeI Zero => new SizeI(0, 0);
        public static SizeI One => new SizeI(1, 1);

        public override string ToString() => $"Width: {Width} Heigth: {Heigth}";

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SizeI))
                return false;

            return this == (SizeI)obj;
        }

        public override int GetHashCode()
        {
            return Width ^ Heigth;
        }

        public override SizeI Clone()
        {
            return new SizeI(Width, Heigth);
        }
    }
}
