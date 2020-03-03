using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    class Point
    {
        public Point()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
        }

        public Point(double _x, double _y)
        {
            x = _x;
            y = _y;
            z = 0.0;
        }

        public Point(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
}
