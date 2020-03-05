using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Geometry;

namespace WindowConfigurator.Interop
{
    class Frame
    {
        #region Properties
        public int id { get; set; }
        public Point start { get; set; }
        public Point end { get; set; }
        public double featurePosition { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        #endregion

        // Counter for auto increment id.
        protected static int globalID = -1;

        /// <summary>
        /// Constructor with nothing, specific for Head, Sill, LeftJamb and RightJamb.
        /// Initialized with width and height in child class.
        /// </summary>
        public Frame() {
            this.id = Interlocked.Increment(ref globalID);
        }

        /// <summary>
        /// Constructor with start and end point for intermediate.
        /// </summary>
        /// <param name="_start">start point, bottom for transom and left for mullion</param>
        /// <param name="_end">end point, top for transom and right for mullion</param>
        public Frame(Point _start, Point _end)
        {
            this.start = _start;
            this.end = _end;
            this.id = Interlocked.Increment(ref globalID);
        }

    }
}
