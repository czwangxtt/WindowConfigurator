using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    public class Mullion : Frame
    {
        public Mullion(Point3 _start, Point3 _end) : base(_start, _end)
        {
            this.keyPosition = this.startPoint.Y;
            this.type = "mullion";
        }
    }
}
