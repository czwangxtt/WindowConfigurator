using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using Newtonsoft.Json;
using WindowConfigurator.Input;
using WindowConfigurator.Module;

namespace WindowConfigurator
{
    public class SetGlazingPanel : Command
    {
        static SetGlazingPanel _instance;
        public SetGlazingPanel()
        {
            _instance = this;
        }

        ///<summary>The only instance of the SetGlazingPanel command.</summary>
        public static SetGlazingPanel Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "SetGlazingPanel"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Guid objGuid;
            ObjRef transomRef;

            using (GetObject getObjectAction = new GetObject())
            {
                getObjectAction.SetCommandPrompt("Please select a glazing panel");
                getObjectAction.GeometryFilter = ObjectType.Brep;
                if (getObjectAction.Get() != GetResult.Object)
                {
                    RhinoApp.WriteLine("No glazing panel was selected.");
                    return getObjectAction.CommandResult();
                }
                transomRef = getObjectAction.Object(0);
                objGuid = getObjectAction.Object(0).ObjectId;
                foreach(ObjRef objRef in getObjectAction.Objects())
                {
                    var obj = objRef.Object();
                    obj.Attributes.ObjectColor = System.Drawing.Color.FromArgb(101, 228, 253);
                    obj.Attributes.ColorSource = ObjectColorSource.ColorFromObject;
                    obj.CommitChanges();
                }
            }

            RhinoApp.WriteLine("Please select a glazing system configuration.");
            var fd = new Rhino.UI.OpenFileDialog { Filter = "TXT Files (*.txt)|*.txt" };
            if (!fd.ShowOpenDialog())
                return Result.Cancel;

            return Result.Success;
        }
    }
}