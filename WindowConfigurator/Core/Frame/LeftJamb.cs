using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Core
{
    public class LeftJamb : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="LeftJamb"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public LeftJamb(double width, double height)
        {
            startPoint = new Point3(0, 0, 0);
            endPoint = new Point3(0, 0, height);
            keyPosition = startPoint.Y;
            type = "leftJamb";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftJamb"/> class.
        /// </summary>
        /// <param name="_start">start point, the bottom for jamb</param>
        /// <param name="_end">end point, the top for jamb</param>
        public LeftJamb(Point3 _start, Point3 _end) : base(_start, _end)
        {
            keyPosition = startPoint.Y;
            type = "leftJamb";
        }
    }
}
