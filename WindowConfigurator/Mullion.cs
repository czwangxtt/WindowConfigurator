using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator
{
    class Mullion : WireFrame
    {
        public Mullion(Point _start, Point _end) : base(_start, _end)
        {
            this.featurePosition = this.start.x;
            this.type = "mullion";
        }
    }
}
