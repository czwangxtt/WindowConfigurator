using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    class Connect
    {
        public int id { get; set; }
        public string connectType { get; set; }
        public Point position { get; set; }
        public int connectFrameId { get; set; }
        

        // Counter for auto increment id.
        protected static int globalId = -1;

        public Connect(Point _position, int _connectFrameId, string _connectType)
        {
            id = Interlocked.Increment(ref globalId);
            position = _position;
            connectFrameId = _connectFrameId;
            connectType = _connectType;
        }

    }
}
