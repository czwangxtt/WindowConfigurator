using System;
using System.Collections.Generic;
using System.Text;

namespace WindowConfiguratorCommnon.Geometry
{
    class Point3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public Point3(double _x, double _y)
        {
            X = _x;
            Y = _y;
            Z = 0.0;
        }

        public Point3(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }
    }
}
