using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPlus.Vectors;

namespace SystemPlus.Path
{
    public class Road : Path
    {
        private Func<float, float> heigthAt;
        Vector2 point1;
        Vector2 point2;

        public Road(Func<float, float> _heigthAt, Vector2 _point1, Vector2 _point2)
        {
            heigthAt = _heigthAt;
            if (_point1.x < _point2.x)
            {
                point1 = _point1;
                point2 = _point2;
            } else
            {
                point1 = _point2;
                point2 = _point1;
            }
        }

        public override IPoint GetPointAt(float percent, float threshold)
        {
            Vector2 point3 = point2 - point1;
            Vector2 point = point3 * (percent / 100f);
            point += point1;

            float terrainY = heigthAt(point.x);
            if (MathPlus.Abs(point.y - terrainY) <= threshold)
                return new RoadPoint(terrainY, RoadPoint.RoadType.Normal);
            else if (point.y < terrainY)
                return new RoadPoint(point.y, RoadPoint.RoadType.Tunnel);
            else
                return new RoadPoint(point.y, RoadPoint.RoadType.Bridge);
        }
    }
}
