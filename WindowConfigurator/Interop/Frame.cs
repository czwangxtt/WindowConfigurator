using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    class Frame
    {
        #region Properties
        public int id { get; protected set; }
        public Point startPoint { get; set; }
        public Point endPoint { get; set; }
        public double keyPosition { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        #endregion

        // Counter for auto increment id.
        protected static int globalID = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        public Frame() {
            id = Interlocked.Increment(ref globalID);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="_start">start point, left for transom and bottom for mullion</param>
        /// <param name="_end">end point, right for transom and top for mullion</param>
        public Frame(Point _start, Point _end)
        {
            startPoint = _start;
            endPoint = _end;
            id = Interlocked.Increment(ref globalID);
        }

    }
}
