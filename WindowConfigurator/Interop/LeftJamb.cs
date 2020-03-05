using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class LeftJamb : Frame
    {
        public LeftJamb(double width, double height)
        {
            this.start = new Point(0, 0, 0);
            this.end = new Point(0, 0, height);
            this.featurePosition = 0;
            this.type = "leftJamb";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftJamb"/> class.
        /// </summary>
        /// <param name="_start">start point, the bottom for jamb</param>
        /// <param name="_end">end point, the top for jamb</param>
        public LeftJamb(Point _start, Point _end) : base(_start, _end)
        {
            this.featurePosition = this.start.x;
            this.type = "leftJamb";
        }
    }
}
