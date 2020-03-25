using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    public class Head : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="Head"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public Head(double width, double height)
        {
            startPoint = new Point3(0, 0, height);
            endPoint = new Point3(0, width, height);
            keyPosition = startPoint.Z;
            type = "head";
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Head"/> class.
        /// </summary>
        /// <param name="_start">start point, the left for head</param>
        /// <param name="_end">end point, the right for head</param>
        public Head(Point3 _start, Point3 _end) : base(_start, _end)
        {
            startPoint = _start;
            endPoint = _end;
            keyPosition = startPoint.Z;
            type = "head";
        }
    }
}
