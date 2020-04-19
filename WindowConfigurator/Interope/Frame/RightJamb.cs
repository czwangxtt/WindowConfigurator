using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    public class RightJamb : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="RightJamb"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public RightJamb(double width, double height)
        {
            startPoint = new Point3(0, width, 0);
            endPoint = new Point3(0, width, height);
            keyPosition = startPoint.Y;
            type = "rightJamb";
        }

        public RightJamb(Point3 _start, Point3 _end) : base(_start, _end)
        {
            keyPosition = startPoint.Y;
            type = "rightJamb";
        }
    }
}
