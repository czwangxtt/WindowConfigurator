﻿using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator
{
    class Transom : WireFrame
    {
        public Transom(Point _start, Point _end) : base(_start, _end)
        {
            this.start = _start;
            this.end = _end;
            this.featurePosition = this.start.y;
            this.type = "transom";
        }
    }
}
