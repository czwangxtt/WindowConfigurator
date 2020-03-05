using System;
using System.Collections.Generic;
using System.Text;

namespace WindowConfigurator.Geometry
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public Point(double _x, double _y)
        {
            X = _x;
            Y = _y;
            Z = 0.0;
        }

        public Point(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }
    }
}
