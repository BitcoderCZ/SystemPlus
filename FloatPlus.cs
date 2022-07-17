using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus
{
    public readonly struct FloatPlus
    {
        //
        // Summary:
        //     Represents the largest possible value of an FloatPlus. This field is constant.
        public static readonly FloatPlus MaxValue = new FloatPlus(9223372036854775807, 0);
        //
        // Summary:
        //     Represents the smallest positive FloatPlus value that is greater than zero.
        //     This field is constant.
        public static readonly FloatPlus Epsilon = new FloatPlus(0, 252);
        //
        // Summary:
        //     Represents the smallest possible value of an FloatPlus. This field is constant.
        public static readonly FloatPlus MinValue = new FloatPlus(-9223372036854775808, 0);
        //
        // Summary:
        //     Represents Infinity (∞). This field is constant.
        public static readonly FloatPlus Infinity = new FloatPlus(255);
        //
        // Summary:
        //     Represents Not a Number (NaN). This field is constant.
        public static readonly FloatPlus NaN = new FloatPlus(253);
        //
        // Summary:
        //     Represents Negative infinity (-∞). This field is constant.
        public static readonly FloatPlus NegativeInfinity = new FloatPlus(254);

        private readonly long numb;
        private readonly byte decPoint;

        public FloatPlus(long _numb, byte _decPoint)
        {
            numb = _numb;
            decPoint = _decPoint;
        }

        private FloatPlus(byte special)
        {
            numb = 0;
            decPoint = special;
        }

        public static FloatPlus Parse(string s)
        {
            if (s == "Nan")
                return NaN;
            else if (s == "Infinity" || s == "∞")
                return Infinity;
            else if (s == "NegativeInfinity" || s == "-∞")
                return NegativeInfinity;

            if (!s.All(c => $"0123456789{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}".Contains(c)) 
                || string.IsNullOrEmpty(s))
                throw new FormatException($"String \"{s}\" was not of format FloatPlus");

            int i;
            for (i = s.Length - 1; i >= 0; i--)
                if (s[i] == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || s[i] != '0')
                {
                    i++;
                    break;
                }

            if (i > s.Length)
                i = s.Length;

            s = s.Substring(0, i);

            return new FloatPlus(long.Parse(s.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "")), 
                (byte)(s.Length - s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) - 1));
        }

        public static bool TryParse(string s, out FloatPlus res)
        {
            res = default;

            if (s == "NaN") {
                res = NaN;
                return true;
            }
            else if (s == "Infinity" || s == "∞") {
                res = Infinity;
                return true;
            }
            else if (s == "NegativeInfinity" || s == "-∞") {
                res = NegativeInfinity;
                return true;
            }

            if (!s.All(c => $"0123456789{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}".Contains(c))
                || string.IsNullOrEmpty(s))
                return false;

            int i;
            for (i = s.Length - 1; i >= 0; i--)
                if (s[i] == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || s[i] != '0')
                {
                    i++;
                    break;
                }

            if (i > s.Length)
                i = s.Length;

            s = s.Substring(0, i);

            res = new FloatPlus(long.Parse(s.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "")),
                (byte)(s.Length - s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) - 1));

            return true;
        }


        /*public static FloatPlus operator *(FloatPlus f1, FloatPlus f2)
            => new FloatPlus(f1.numb * f2.numb, (byte)(f2.decPoint + f2.decPoint));*/
        public static FloatPlus operator *(FloatPlus f1, FloatPlus f2) 
            => new FloatPlus(f1.numb * f2.numb, (byte)(f2.decPoint + f2.decPoint));

        public static implicit operator FloatPlus(float f) => Parse(f.ToString());

        public override string ToString()
        {
            if (decPoint == 253)
                return "NaN";
            else if (decPoint == 254)
                return "";

            string s = numb.ToString();

            if (decPoint == 0)
                return s;
            else if (s.Length - decPoint > 0)
                return s.Insert((int)(s.Length - decPoint), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            else
            {
                int max = (int)Math.Abs(s.Length - decPoint) + 1;
                for (int i = 0; i < max; i++)
                    s = "0" + s;
                return s.Insert((int)(s.Length - decPoint), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
        }
    }
}
