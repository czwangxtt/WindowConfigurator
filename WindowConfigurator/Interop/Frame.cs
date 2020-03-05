using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Geometry;

namespace WindowConfigurator.Interop
{
    class Frame
    {

        protected int id { get; set; }
        protected Point start { get; set; }
        protected Point end { get; set; }
        protected double featurePosition { get; set; }
        protected string type { get; set; }
        protected int level { get; set; }

        protected static int globalID = -1;
        
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
