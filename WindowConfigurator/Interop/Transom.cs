using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator.Interop
{
    class Transom : Frame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transom"/> class.
        /// </summary>
        /// <param name="_start">start point, bottom for transom</param>
        /// <param name="_end">end point, top for transom</param>
        public Transom(Point _start, Point _end) : base(_start, _end)
        {
            this.start = _start;
            this.end = _end;
            this.featurePosition = this.start.y;
            this.type = "transom";
        }
    }
}
