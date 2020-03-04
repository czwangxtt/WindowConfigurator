using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class RightJamb : Frame
    {
        public RightJamb(double width, double height)
        {
            this.start = new Point(0, width, 0);
            this.end = new Point(0, width, height);
            this.featurePosition = width;
            this.type = "rightJamb";
        }

        public RightJamb(Point _start, Point _end) : base(_start, _end)
        {
            this.featurePosition = this.start.x;
            this.type = "rightJamb";
        }
    }
}
