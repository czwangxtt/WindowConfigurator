using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    public class Transom : Frame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transom"/> class.
        /// </summary>
        /// <param name="_start">start point, the left for transom</param>
        /// <param name="_end">end point, the right for transom</param>
        public Transom(Point3 _start, Point3 _end) : base(_start, _end)
        {
            startPoint = _start;
            endPoint = _end;
            keyPosition = startPoint.Z;
            type = "transom";
        }
    }
}
