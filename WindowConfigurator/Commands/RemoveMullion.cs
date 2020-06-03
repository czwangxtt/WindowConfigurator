using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using WindowConfigurator.Core;

namespace WindowConfigurator
{
    public class RemoveMullion : Command
    {
        static RemoveMullion _instance;
        public RemoveMullion()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RemoveMullion command.</summary>
        public static RemoveMullion Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RemoveMullion"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Guid objGuid;
            ObjRef mullionRef;

            using (GetObject getObjectAction = new GetObject())
            {
                getObjectAction.SetCommandPrompt("Please select a mullion");
                getObjectAction.GeometryFilter = ObjectType.Curve;
                if (getObjectAction.Get() != GetResult.Object)
                {
                    RhinoApp.WriteLine("No mullion was selected.");
                    return getObjectAction.CommandResult();
                }
                if (getObjectAction.Objects().Length > 1)
                {
                    RhinoApp.WriteLine("Please select one object each time");
                    return getObjectAction.CommandResult();
                }
                mullionRef = getObjectAction.Object(0);
                objGuid = getObjectAction.Object(0).ObjectId;
                
            }

            Mullion mullion = InitializeWindow.window.wireFrame.GetMullionByGuid(objGuid);
            List<Guid> updatedTransom = InitializeWindow.window.wireFrame.removeIntermediate(mullion);

            foreach (var guid in updatedTransom)
            {
                Transom transom = InitializeWindow.window.wireFrame.GetTransomByGuid(guid);
                Point3d pt0 = new Point3d(transom.startPoint.X, transom.startPoint.Y, transom.startPoint.Z);
                Point3d pt1 = new Point3d(transom.endPoint.X, transom.endPoint.Y, transom.endPoint.Z);
                Line newTransom = new Line(pt0, pt1);

                double offset0 = 0.0;
                double offset1 = 0.0;
                if (pt0.Y == 0)
                    offset0 = 59;
                else
                    offset0 = 38.5;
                if (pt1.Y == 1500)
                    offset1 = -59;
                else
                    offset1 = -38.5;
                Curve rail_crv = new Line(new Point3d(pt0.X, pt0.Y + offset0, pt0.Z), new Point3d(pt1.X, pt1.Y + offset1, pt1.Z)).ToNurbsCurve();
                var breps = Brep.CreateFromSweep(rail_crv, transom.cross_section, true, doc.ModelAbsoluteTolerance);
                doc.Objects.Replace(transom.extrusionGuid, breps[0]);
                doc.Objects.Replace(guid, newTransom);
            }

            doc.Objects.Delete(mullionRef, true, true);
            doc.Objects.Delete(mullion.extrusionGuid, true);

            foreach(var glazingPanelGuid in mullion.glazingPanelGuids)
            {
                doc.Objects.Delete(glazingPanelGuid, true);
            }

            foreach (var connection in mullion.Connects)
            {
                doc.Objects.Delete(connection.guid, true);
            }
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command removed a mullion in the document.", EnglishName);

            return Result.Success;
        }
    }
}
