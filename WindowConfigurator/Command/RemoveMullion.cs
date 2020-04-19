using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;


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
            ObjRef mullion;

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
                mullion = getObjectAction.Object(0);
                objGuid = getObjectAction.Object(0).ObjectId;
                
            }


            doc.Objects.Delete(mullion, true, true);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command removed one mullion to the document.", EnglishName);

            return Result.Success;
        }
    }
}
