using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Core
{
    public class Field
    {
        public int id { get; protected set; }
        public Guid guid { get; set; }
        public Boolean isVisible { get; set; }
        public GlazingPanel glazingPanel { get; set; }
        public List<int> frameIds { get; set; }
        public List<Point3> vertics { get; set; }
        public List<double> keyPosition { get; set; }

        // Counter for auto increment id.
        protected static int globalID = -1;

        public Field(Guid _guid)
        {
            id = Interlocked.Increment(ref globalID);
            isVisible = true;
            guid = _guid;
        }
    }
}
