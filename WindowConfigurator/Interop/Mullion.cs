using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    class Mullion : Frame
    {
        public Mullion(Point _start, Point _end) : base(_start, _end)
        {
            this.keyPosition = this.startPoint.Y;
            this.type = "mullion";
        }
    }
}
