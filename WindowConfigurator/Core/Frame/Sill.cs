using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Core
{
    public class Sill : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="Sill"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public Sill(double width, double height)
        {
            startPoint = new Point3(0, 0, 0);
            endPoint = new Point3(0, width, 0);
            keyPosition = startPoint.Z;
            type = "sill";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sill"/> class.
        /// </summary>
        /// <param name="_start">start point, the left for sill</param>
        /// <param name="_end">end point, the right for sill</param>
        public Sill(Point3 _start, Point3 _end) : base(_start, _end)
        {
            startPoint = _start;
            endPoint = _end;
            keyPosition = startPoint.Z;
            type = "sill";
        }
    }
}
