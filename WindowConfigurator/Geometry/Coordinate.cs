﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WindowConfiguratorCommnon.Geometry
{
    class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Coordinate(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
