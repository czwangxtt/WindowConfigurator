using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    public class Frame
    {
        #region Properties
        public int id { get; protected set; }
        public Guid guid { get; set; }
        public string type { get; set; }
        public Point3 startPoint { get; set; }
        public Point3 endPoint { get; set; }
        public double keyPosition { get; set; }
        public int level { get; set; }
        public Boolean isVisible { get; set; }

        private List<Connect> _connects = new List<Connect>();
        #endregion

        // Counter for auto increment id.
        protected static int globalID = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        public Frame() {
            id = Interlocked.Increment(ref globalID);
            isVisible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="_start">start point, left for transom and bottom for mullion</param>
        /// <param name="_end">end point, right for transom and top for mullion</param>
        public Frame(Point3 _start, Point3 _end)
        {
            startPoint = _start;
            endPoint = _end;
            id = Interlocked.Increment(ref globalID);
            isVisible = true;
        }

        public List<Connect> Connects
        {
            get { return _connects; }
        }

        public void AddConnect(Connect connect)
        {
            _connects.Add(connect);
        }

    }
}
