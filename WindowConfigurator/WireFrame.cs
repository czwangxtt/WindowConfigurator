using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace WindowConfigurator
{
    class WireFrame
    {
        public WireFrame(Point _start, Point _end)
        {
            this.start = _start;
            this.end = _end;
        }

        public int id { get; set; }
        public Point start { get; set; }
        public Point end { get; set; }
        public double featurePosition { get; set; }
        public string type { get; set; }
        public int level { get; set; }

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
