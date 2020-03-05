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

            horizontalFrames.Add(head.featurePosition, head.id);
            horizontalFrames.Add(sill.featurePosition, sill.id);

            verticalFrames.Add(leftJamb.featurePosition, leftJamb.id);
            verticalFrames.Add(rightJamb.featurePosition, rightJamb.id);
        }

        public void AddIntermediate(Transom transom)
        {
            allFrames.Add(transom);
            horizontalFrames.Add(transom.featurePosition, transom.id);

        }

        public void AddIntermediate(Mullion mullion)
        {
            allFrames.Add(mullion);
            verticalFrames.Add(mullion.featurePosition, mullion.id);
        }

        public void RemoveIntermediate(Transom transom)
        {

        }

        public void RemoveIntermediate(Mullion mullion)
        {

        }


    }
}
