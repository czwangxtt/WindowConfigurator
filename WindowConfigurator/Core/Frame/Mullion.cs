using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Core
{
    public class Mullion : Frame
    {
        public Mullion(Point3 _start, Point3 _end) : base(_start, _end)
        {
            startPoint = _start;
            endPoint = _end;
            this.keyPosition = this.startPoint.Y;
            this.type = "mullion";
        }

        public Mullion(Point3 _start, Point3 _end, Guid _guid, Guid _extrusionGuid) : base(_start, _end)
        {
            guid = _guid;
            extrusionGuid = _extrusionGuid;
            startPoint = _start;
            endPoint = _end;
            this.keyPosition = this.startPoint.Y;
            this.type = "mullion";
        }
    }
}
