using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus
{
    public sealed class RandomPlus
    {
        public RandomPlus(long seed)
        {
            _seed = (seed ^ LARGE_PRIME) & ((1L << 48) - 1);
        }

        public void setSeed(long seed)
        {
            _seed = (seed ^ LARGE_PRIME) & ((1L << 48) - 1);
        }

        private int next(int bits)
        {
            _seed = (_seed * LARGE_PRIME + SMALL_PRIME) & ((1L << 48) - 1);
            return (int)(((uint)_seed) >> (48 - bits));
        }

        public void nextBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                for (int rnd = nextInt(), n = Math.Min(bytes.Length - i, 4);
                     n-- > 0; rnd >>= 8)
                    bytes[i++] = (byte)rnd;
        }

        public int nextInt()
        {
            return next(32);
        }

        public int nextInt(int n)
        {
            if (n <= 0)
                throw new ArgumentOutOfRangeException("n", n, "n must be positive");

            if ((n & -n) == n)  // i.e., n is a power of 2
                return (int)((n * (long)next(31)) >> 31);

            int bits, val;

            do
            {
                bits = next(31);
                val = bits % n;
            } while (bits - val + (n - 1) < 0);
            return val;
        }

        public long nextLong()
        {
            return ((long)next(32) << 32) + next(32);
        }

        public bool nextBoolean()
        {
            return next(1) != 0;
        }

        public float nextFloat()
        {
            return next(24) / ((float)(1 << 24));
        }

        public double nextDouble()
        {
            return (((long)next(26) << 27) + next(27))
              / (double)(1L << 53);
        }

        private double nextNextGaussian;
        private bool haveNextNextGaussian = false;

        public double nextGaussian()
        {
            if (haveNextNextGaussian)
            {
                haveNextNextGaussian = false;
                return nextNextGaussian;
            }
            else
            {
                double v1, v2, s;
                do
                {
                    v1 = 2 * nextDouble() - 1;   // between -1.0 and 1.0
                    v2 = 2 * nextDouble() - 1;   // between -1.0 and 1.0
                    s = v1 * v1 + v2 * v2;
                } while (s >= 1 || s == 0);
                double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
                nextNextGaussian = v2 * multiplier;
                haveNextNextGaussian = true;
                return v1 * multiplier;
            }
        }

        private long _seed;

        private const long LARGE_PRIME = 0x5DEECE66DL;
        private const long SMALL_PRIME = 0xBL;
    }
}
