using System;

namespace SystemPlus.Vectors
{
    struct Point3D
    {
        public double x;
        public double y;
        public double z;

        public double u;
        public double v;
        public double w;

        public Point3D(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
            u = 0;
            v = 0;
            w = 1;
        }

        public Point3D(double _x, double _y, double _z, double _u, double _v)
        {
            x = _x;
            y = _y;
            z = _z;
            u = _u;
            v = _v;
            w = 1;
        }

        public static Point3D Translate(Point3D original, Point3D translation)
        {
            Point3D toReturn;
            toReturn.x = original.x + translation.x;
            toReturn.y = original.y + translation.y;
            toReturn.z = original.z + translation.z;
            toReturn.u = original.u;
            toReturn.v = original.v;
            toReturn.w = original.w;
            return toReturn;
        }

        public static Point3D Rotate(Point3D original, Point3D rotation)
        {
            Point3D toReturn;
            // Rotation matrix: https://en.wikipedia.org/wiki/Rotation_matrix
            toReturn.x = original.x * (Math.Cos(rotation.z) * Math.Cos(rotation.y)) +
                         original.y * (Math.Cos(rotation.z) * Math.Sin(rotation.y) * Math.Sin(rotation.x) -
                         Math.Sin(rotation.z) * Math.Cos(rotation.x)) +
                         original.z * (Math.Cos(rotation.z) * Math.Sin(rotation.y) * Math.Cos(rotation.x) +
                         Math.Sin(rotation.z) * Math.Sin(rotation.x));
            toReturn.y = original.x * (Math.Sin(rotation.z) * Math.Cos(rotation.y)) +
                         original.y * (Math.Sin(rotation.z) * Math.Sin(rotation.y) * Math.Sin(rotation.x) +
                         Math.Cos(rotation.z) * Math.Cos(rotation.x)) +
                         original.z * (Math.Sin(rotation.z) * Math.Sin(rotation.y) * Math.Cos(rotation.x) -
                         Math.Cos(rotation.z) * Math.Sin(rotation.x));
            toReturn.z = original.x * (-Math.Sin(rotation.y)) +
                         original.y * (Math.Cos(rotation.y) * Math.Sin(rotation.x)) +
                         original.z * (Math.Cos(rotation.y) * Math.Cos(rotation.x));
            toReturn.u = original.u;
            toReturn.v = original.v;
            toReturn.w = original.w;
            return toReturn;
        }
    }
}
