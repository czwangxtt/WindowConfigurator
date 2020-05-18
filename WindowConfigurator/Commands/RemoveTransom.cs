using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using WindowConfigurator.Interop;


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
                doc.Objects.Replace(guid, newMullion);
            }

            doc.Objects.Delete(transomRef, true, true);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command removed a transom in the document.", EnglishName);

            return Result.Success;
        }
    }
}