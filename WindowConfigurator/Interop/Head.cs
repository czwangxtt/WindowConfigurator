using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class  Head : Frame
    {
        public Head(double width, double height)
        {
            this.start = new Point(0, 0, height);
            this.end = new Point(0, width, height);
            this.featurePosition = height;
            this.type = "head";
        }

        public Head(Point _start, Point _end) : base(_start, _end)
        {
            this.start = _start;
            this.end = _end;
            this.featurePosition = this.start.y;
            this.type = "head";
        }
    }
}
