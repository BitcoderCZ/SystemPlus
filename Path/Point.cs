using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus.Path
{
    public abstract class Point : IPoint
    {
        public float Heigth => heigth;

        private readonly float heigth;

        public Point(float _heigth)
        {
            heigth = _heigth;
        }
    }
}
