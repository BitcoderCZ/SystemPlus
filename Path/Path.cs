using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus.Path
{
    public abstract class Path
    {
        public abstract IPoint GetPointAt(float percent, float threshold);
    }
}
