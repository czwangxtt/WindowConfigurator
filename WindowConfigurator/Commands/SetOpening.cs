using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace WindowConfigurator.Commands
{
    public class SetOpening : Command
    {
        static SetOpening _instance;
        public SetOpening()
        {
            _instance = this;
        }

        ///<summary>The only instance of the SetOpening command.</summary>
        public static SetOpening Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "SetOpening"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            using (GetString getArticleNumberAction = new GetString())
            {
                getArticleNumberAction.SetCommandPrompt("Please select openning type, 0 for inward and 1 for outward.");
                if (getArticleNumberAction.Get() != GetResult.String)
                {
                    RhinoApp.WriteLine("Please type 0 or 1.");
                    return getArticleNumberAction.CommandResult();
                }

            }

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
                foreach (ObjRef objRef in getObjectAction.Objects())
                {
                    var obj = objRef.Object();
                    obj.Attributes.ObjectColor = System.Drawing.Color.FromArgb(101, 228, 253);
                    obj.Attributes.ColorSource = ObjectColorSource.ColorFromObject;
                    obj.CommitChanges();
                }
            }


            return Result.Success;
        }
    }
}