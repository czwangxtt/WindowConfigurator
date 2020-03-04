using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Geometry;

namespace WindowConfigurator.Interop
{
    class Frame
    {

        public int id { get; protected set; }
        public Point start { get; set; }
        public Point end { get; set; }
        public double featurePosition { get; set; }
        public string type { get; set; }
        public int level { get; set; }

        public static int globalID = -1;
        
        public Frame()
        {

        }

        public Frame(Point _start, Point _end)
        {
            this.start = _start;
            this.end = _end;
            this.id = Interlocked.Increment(ref globalID);
        }

        public int getId()
        {
            return id;
        }

        public Point getStartPoint()
        {
            return start;
        }

        public Point getEndPoint()
        {
            return end;
        }

        public double getFeaturePosition()
        {
            return featurePosition;
        }

        public string getType()
        {
            return type;
        }

        public int getLevel()
        {
            return level;
        }

    }
}
