using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WindowConfigurator.Interop
{
    class WireFrame
    {
        private SortedList<double, int> horzIdBySortedkeyPosition = new SortedList<double, int>();
        private SortedList<double, int> vrtIdBySortedkeyPosition = new SortedList<double, int>();
        private List<Frame> allFrames = new List<Frame>();


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

            horzIdBySortedkeyPosition.Add(head.keyPosition, head.id);
            horzIdBySortedkeyPosition.Add(sill.keyPosition, sill.id);

            vrtIdBySortedkeyPosition.Add(leftJamb.keyPosition, leftJamb.id);
            vrtIdBySortedkeyPosition.Add(rightJamb.keyPosition, rightJamb.id);
        }

        public void UpdateConnection()
        {

        }

        /// <summary>
        /// Adds a transom to the window system.
        /// </summary>
        /// <param name="transom">the new transom object</param>
        public void addIntermediate(Transom transom)
        {
            allFrames.Add(transom);
            horzIdBySortedkeyPosition.Add(transom.keyPosition, transom.id);

        }


        /// <summary>
        /// Adds a mullion to the window system.
        /// </summary>
        /// <param name="mullion">the new mullion object</param>
        public void addIntermediate(Mullion mullion)
        {
            allFrames.Add(mullion);
            vrtIdBySortedkeyPosition.Add(mullion.keyPosition, mullion.id);
        }


        //TODO: Add remove intermediate function 
        public void removeIntermediate(Transom transom)
        {

        }

        public void removeIntermediate(Mullion mullion)
        {
        }


    }
}
