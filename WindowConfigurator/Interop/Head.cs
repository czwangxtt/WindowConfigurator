using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class  Head : Frame
    {
        /// <summary>
        /// Initializes a new instance of th e<see cref="Head"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
        public Head(double width, double height)
        {
            startPoint = new Point(0, 0, height);
            endPoint = new Point(0, width, height);
            featurePosition = height;
            type = "head";
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Head"/> class.
        /// </summary>
        /// <param name="_start">start point, the left for head</param>
        /// <param name="_end">end point, the right for head</param>
        public Head(Point _start, Point _end) : base(_start, _end)
        {
            startPoint = _start;
            endPoint = _end;
            featurePosition = this.startPoint.y;
            type = "head";
        }
    }
}
