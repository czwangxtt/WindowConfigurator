using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WindowConfigurator.Interop
{
    class WireFrame
    {
        public SortedList<double, int> horizontalFrames { get; set; }
        public SortedList<double, int> verticalFrames { get; set; }
        public List<Frame> allFrames { get; set; }


        public WireFrame(double width, double height)
        {
            Head head = new Head(width, height);
            Sill sill = new Sill(width, height);
            LeftJamb leftJamb = new LeftJamb(width, height);
            RightJamb rightJamb = new RightJamb(width, height);

            allFrames.Add(head);
            allFrames.Add(sill);
            allFrames.Add(leftJamb);
            allFrames.Add(rightJamb);

            horizontalFrames.Add(head.getFeaturePosition(), head.getId());
            horizontalFrames.Add(sill.getFeaturePosition(), sill.getId());

            verticalFrames.Add(leftJamb.getFeaturePosition(), leftJamb.getId());
            verticalFrames.Add(rightJamb.getFeaturePosition(), rightJamb.getId());
        }
    }
}
