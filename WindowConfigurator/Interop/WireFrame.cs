using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WindowConfigurator.Interop
{
    class WireFrame
    {
        public SortedList<double, int> horizontalFrames = new SortedList<double, int>();
        public SortedList<double, int> verticalFrames = new SortedList<double, int>();
        public List<Frame> allFrames = new List<Frame>();


        /// <summary>
        /// Initializes a new instance of the <see cref="WireFrame"/> class.
        /// </summary>
        /// <param name="width">the outer width of the window</param>
        /// <param name="height">the outer height of the window</param>
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


        /// <summary>
        /// Adds a transom to the window system.
        /// </summary>
        /// <param name="transom">the new transom object</param>
        public void AddIntermediate(Transom transom)
        {
            allFrames.Add(transom);
            horizontalFrames.Add(transom.featurePosition, transom.id);

        }


        /// <summary>
        /// Adds a mullion to the window system.
        /// </summary>
        /// <param name="mullion">the new mullion object</param>
        public void AddIntermediate(Mullion mullion)
        {
            allFrames.Add(mullion);
            verticalFrames.Add(mullion.featurePosition, mullion.id);
        }


        //TODO: Add remove intermediate function 
        public void RemoveIntermediate(Transom transom)
        {

        }

        public void RemoveIntermediate(Mullion mullion)
        {
        }


    }
}
