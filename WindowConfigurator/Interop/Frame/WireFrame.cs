using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WindowConfigurator.Geometry;

namespace WindowConfigurator.Interop
{
    class WireFrame
    {
        //TODO Change data type since SortedList doesn't allow duplication.
        private SortedList<double, int> horzIdBySortedkeyPosition = new SortedList<double, int>();
        private SortedList<double, int> vrtIdBySortedkeyPosition = new SortedList<double, int>();
        private List<Frame> _frames = new List<Frame>();


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

            _frames.Add(head);
            _frames.Add(sill);
            _frames.Add(leftJamb);
            _frames.Add(rightJamb);

            horzIdBySortedkeyPosition.Add(head.keyPosition, head.id);
            horzIdBySortedkeyPosition.Add(sill.keyPosition, sill.id);

            vrtIdBySortedkeyPosition.Add(leftJamb.keyPosition, leftJamb.id);
            vrtIdBySortedkeyPosition.Add(rightJamb.keyPosition, rightJamb.id);

            head.AddConnect(new Connect(rightJamb.endPoint, rightJamb.id, "corner"));
            rightJamb.AddConnect(new Connect(sill.endPoint, sill.id, "corner"));
            sill.AddConnect(new Connect(leftJamb.startPoint, leftJamb.id, "corner"));
            leftJamb.AddConnect(new Connect(head.startPoint, head.id, "corner"));
        }

        /// <summary>
        /// Return frames list
        /// </summary>
        public List<Frame> Frames
        {
            get { return _frames; }
        }

        public SortedList<double, int> hFrames
        {
            get { return horzIdBySortedkeyPosition; }
        }

        public SortedList<double, int> vFrames
        {
            get { return vrtIdBySortedkeyPosition; }
        }


        /// <summary>
        /// Updates the connection when adding a transom to window system.
        /// </summary>
        /// <param name="transom">A new transom added</param>
        public void UpdateConnection(Transom transom)
        {
            Point startPt = transom.startPoint;
            Point endPt = transom.endPoint;
            transom.AddConnect(new Connect(startPt, vrtIdBySortedkeyPosition[startPt.Y], "tee"));
            transom.AddConnect(new Connect(endPt, vrtIdBySortedkeyPosition[endPt.Y], "tee"));
        }


        /// <summary>
        /// Updates the connection when adding a mullion to window system.
        /// </summary>
        /// <param name="mullion">A new mullion added</param>
        public void UpdateConnection(Mullion mullion)
        {
            Point startPt = mullion.startPoint;
            Point endPt = mullion.endPoint;
            mullion.AddConnect(new Connect(startPt, vrtIdBySortedkeyPosition[startPt.Y], "tee"));
            mullion.AddConnect(new Connect(endPt, vrtIdBySortedkeyPosition[endPt.Y], "tee"));
        }


        /// <summary>
        /// Adds a transom to the window system.
        /// </summary>
        /// <param name="transom">the new transom object</param>
        public void addIntermediate(Transom transom)
        {
            _frames.Add(transom);
            horzIdBySortedkeyPosition.Add(transom.keyPosition, transom.id);
            UpdateConnection(transom);
        }


        /// <summary>
        /// Adds a mullion to the window system.
        /// </summary>
        /// <param name="mullion">the new mullion object</param>
        public void addIntermediate(Mullion mullion)
        {
            _frames.Add(mullion);
            vrtIdBySortedkeyPosition.Add(mullion.keyPosition, mullion.id);
            UpdateConnection(mullion);
        }


        /// <summary>
        /// Removes transom from the frames and extends all mullions ending at that transom to the next one.
        /// </summary>
        /// <param name="transom">the transom need to be removed</param>
        public void removeIntermediate(Transom transom)
        {
            int removeId = transom.id;
            foreach (var frame in _frames)
            {
                foreach (var connect in frame.Connects)
                {
                    if (connect.connectFrameId == removeId) {
                        frame.Connects.Remove(connect);
                    }
                }
            }

            int horzTransomIndex = horzIdBySortedkeyPosition.IndexOfKey(transom.keyPosition);
            foreach (var mullionId in vrtIdBySortedkeyPosition.Values)
            {
                if (_frames[mullionId].startPoint.Y == transom.keyPosition)
                    _frames[mullionId].startPoint.Y = _frames[horzIdBySortedkeyPosition[horzTransomIndex - 1]].keyPosition;
                else if(_frames[mullionId].endPoint.Y == transom.keyPosition)
                    _frames[mullionId].endPoint.Y = _frames[horzIdBySortedkeyPosition[horzTransomIndex + 1]].keyPosition;
            }

            horzIdBySortedkeyPosition.Remove(transom.keyPosition);
            _frames.Remove(transom);
        }


        /// <summary>
        /// Removes mullion from the frames and extends all transoms ending at that mullion to the next one.
        /// </summary>
        /// <param name="mullion">the mullion need to be removed</param>
        public void removeIntermediate(Mullion mullion)
        {
            int removeId = mullion.id;
            foreach (var frame in _frames)
            {
                foreach (var connect in frame.Connects)
                {
                    if (connect.connectFrameId == removeId)
                    {
                        frame.Connects.Remove(connect);
                    }
                }
            }

            int vrtTransomIndex = vrtIdBySortedkeyPosition.IndexOfKey(mullion.keyPosition);
            foreach (var transomId in horzIdBySortedkeyPosition.Values)
            {
                if (_frames[transomId].startPoint.Z == mullion.keyPosition)
                    _frames[transomId].startPoint.Z = _frames[vrtIdBySortedkeyPosition[vrtTransomIndex - 1]].keyPosition;
                else if (_frames[transomId].endPoint.Z == mullion.keyPosition)
                    _frames[transomId].endPoint.Z = _frames[vrtIdBySortedkeyPosition[vrtTransomIndex + 1]].keyPosition;
            }

            horzIdBySortedkeyPosition.Remove(mullion.keyPosition);
            _frames.Remove(mullion);
        }


    }
}
