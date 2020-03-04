using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class  Sill : Frame
    {
        public Sill(double width, double height)
        {
            this.start = new Point(0, 0, 0);
            this.end = new Point(0, width, 0);
            this.featurePosition = 0;
            this.type = "sill";
        }

        public Sill(Point _start, Point _end) : base(_start, _end)
        {
            this.start = _start;
            this.end = _end;
            this.featurePosition = this.start.y;
            this.type = "sill";
        }
    }
}
