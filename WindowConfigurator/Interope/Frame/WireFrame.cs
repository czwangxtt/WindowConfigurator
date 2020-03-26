using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WindowConfigurator.Geometry;
using WindowConfigurator.Utilities;

namespace WindowConfigurator.Interop
{
    class WireFrame
    {
        private SortedMultiValue<double, int> horzIdBySortedkeyPosition = new SortedMultiValue<double, int>();
        private SortedMultiValue<double, int> vrtIdBySortedkeyPosition = new SortedMultiValue<double, int>();
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

        public SortedMultiValue<double, int> hFrames
        {
            get { return horzIdBySortedkeyPosition; }
        }

        public SortedMultiValue<double, int> vFrames
        {
            get { return vrtIdBySortedkeyPosition; }
        }


        /// <summary>
        /// Updates the connection when adding a transom to window system.
        /// </summary>
        /// <param name="transom">A new transom added</param>
        public void UpdateConnection(Transom transom)
        {
            Point3 startPt = transom.startPoint;
            Point3 endPt = transom.endPoint;
            foreach (int id in vrtIdBySortedkeyPosition.Get(startPt.Y))
            {
                if (transom.keyPosition > _frames[id].startPoint.Z && transom.keyPosition < _frames[id].endPoint.Z)
                    transom.AddConnect(new Connect(startPt, id, "tee"));
            }
            foreach (int id in vrtIdBySortedkeyPosition.Get(endPt.Y))
            {
                if (transom.keyPosition > _frames[id].startPoint.Z && transom.keyPosition < _frames[id].endPoint.Z)
                    transom.AddConnect(new Connect(endPt, id, "tee"));
            }
        }


        /// <summary>
        /// Updates the connection when adding a mullion to window system.
        /// </summary>
        /// <param name="mullion">A new mullion added</param>
        public void UpdateConnection(Mullion mullion)
        {
            Point3 startPt = mullion.startPoint;
            Point3 endPt = mullion.endPoint;

            foreach (int id in horzIdBySortedkeyPosition.Get(startPt.Z))
            {
                if (mullion.keyPosition > _frames[id].startPoint.Y && mullion.keyPosition < _frames[id].endPoint.Y)
                    mullion.AddConnect(new Connect(startPt, id, "tee"));
            }
            foreach (int id in horzIdBySortedkeyPosition.Get(endPt.Z))
            {
                if (mullion.keyPosition > _frames[id].startPoint.Y && mullion.keyPosition < _frames[id].endPoint.Y)
                    mullion.AddConnect(new Connect(endPt, id, "tee"));
            }
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
                foreach (var connect in frame.Connects.ToList())
                {
                    if (connect.connectFrameId == removeId)
                    {
                        frame.Connects.Remove(connect);
                    }
                }
            }

            // Extend mullions ending at the removing transom
            int horzTransomIndex = horzIdBySortedkeyPosition.IndexOf(transom.id);
            foreach (var mullionId in vrtIdBySortedkeyPosition)
            {
                if (_frames[mullionId].startPoint.Z == transom.keyPosition)
                    _frames[mullionId].startPoint.Z = _frames[horzIdBySortedkeyPosition.ElementAt(horzTransomIndex - 1)].keyPosition;
                else if (_frames[mullionId].endPoint.Z == transom.keyPosition)
                    _frames[mullionId].endPoint.Z = _frames[horzIdBySortedkeyPosition.ElementAt(horzTransomIndex + 1)].keyPosition;
            }

            horzIdBySortedkeyPosition.Remove(transom.keyPosition, transom.id);
            transom.isVisible = false;
            //intermediate should not been removed since the user may want to redo it.
            //_frames.Remove(transom); 
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
                foreach (var connect in frame.Connects.ToList())
                {
                    if (connect.connectFrameId == removeId)
                    {
                        frame.Connects.Remove(connect);
                    }
                }
            }

            // Extend transoms ending at the removing mullion
            int vrtTransomIndex = vrtIdBySortedkeyPosition.IndexOf(mullion.id);
            foreach (var transomId in horzIdBySortedkeyPosition)
            {
                {
                    if (_frames[transomId].startPoint.Y == mullion.keyPosition)
                        _frames[transomId].startPoint.Y = _frames[vrtIdBySortedkeyPosition.ElementAt(vrtTransomIndex - 1)].keyPosition;
                    else if (_frames[transomId].endPoint.Y == mullion.keyPosition)
                        _frames[transomId].endPoint.Y = _frames[vrtIdBySortedkeyPosition.ElementAt(vrtTransomIndex + 1)].keyPosition;
                }

            }
            horzIdBySortedkeyPosition.Remove(mullion.keyPosition, mullion.id);
            mullion.isVisible = false;
            //intermediate should not been removed since the user may want to redo it.
            //_frames.Remove(mullion);
        }
    }
}
