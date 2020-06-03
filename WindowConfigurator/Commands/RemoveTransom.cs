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
    public class RemoveTransom : Command
    {
        static RemoveTransom _instance;
        public RemoveTransom()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RemoveTransom command.</summary>
        public static RemoveTransom Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RemoveTransom"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Guid objGuid;
            ObjRef transomRef;

            using (GetObject getObjectAction = new GetObject())
            {
                getObjectAction.SetCommandPrompt("Please select a transom");
                getObjectAction.GeometryFilter = ObjectType.Curve;
                if (getObjectAction.Get() != GetResult.Object)
                {
                    RhinoApp.WriteLine("No transom was selected.");
                    return getObjectAction.CommandResult();
                }
                if (getObjectAction.Objects().Length > 1)
                {
                    RhinoApp.WriteLine("Please select one object each time");
                    return getObjectAction.CommandResult();
                }
                transomRef = getObjectAction.Object(0);
                objGuid = getObjectAction.Object(0).ObjectId;
            }

            Transom transom = InitializeWindow.window.wireFrame.GetTransomByGuid(objGuid);
            List<Guid> updatedMullion = InitializeWindow.window.wireFrame.removeIntermediate(transom);

            foreach (var guid in updatedMullion)
            {
                Mullion mullion = InitializeWindow.window.wireFrame.GetMullionByGuid(guid);
                Point3d pt0 = new Point3d(mullion.startPoint.X, mullion.startPoint.Y, mullion.startPoint.Z);
                Point3d pt1 = new Point3d(mullion.endPoint.X, mullion.endPoint.Y, mullion.endPoint.Z);
                Line newMullion = new Line(pt0, pt1);

                double offset0 = 0.0;
                double offset1 = 0.0;
                if (pt0.Z == 0)
                    offset0 = 59;
                else
                    offset0 = 38.5;
                if (pt1.Z == 1000)
                    offset1 = -59;
                else
                    offset1 = -38.5;
                Curve rail_crv = new Line(new Point3d(pt0.X, pt0.Y, pt0.Z + offset0), new Point3d(pt1.X, pt1.Y, pt1.Z + offset1)).ToNurbsCurve();
                var breps = Brep.CreateFromSweep(rail_crv, mullion.cross_section, true, doc.ModelAbsoluteTolerance);
                doc.Objects.Replace(mullion.extrusionGuid, breps[0]);
                doc.Objects.Replace(guid, newMullion);
            }

            doc.Objects.Delete(transomRef, true, true);
            doc.Objects.Delete(transom.extrusionGuid, true);

            foreach (var glazingPanelGuid in transom.glazingPanelGuids)
            {
                doc.Objects.Delete(glazingPanelGuid, true);
            }

            foreach (var connection in transom.Connects)
            {
                doc.Objects.Delete(connection.guid, true);
            }
            
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command removed a transom in the document.", EnglishName);

            return Result.Success;
        }
    }
}