using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    class RightJamb : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="RightJamb"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public RightJamb(double width, double height)
        {
            startPoint = new Point(0, width, 0);
            endPoint = new Point(0, width, height);
            keyPosition = startPoint.X;
            type = "rightJamb";
        }

        public RightJamb(Point _start, Point _end) : base(_start, _end)
        {
            keyPosition = startPoint.X;
            type = "rightJamb";
        }
    }
}
