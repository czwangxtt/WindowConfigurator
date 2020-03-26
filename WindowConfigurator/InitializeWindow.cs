using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace WindowConfigurator
{
    public class InitializeWindow : Command
    {
        public InitializeWindow()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static InitializeWindow Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "InitializeWindow"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will create a wireframe right now.", EnglishName);

            double width;
            Point3d pt0;
            Point3d pt1;
            using (GetNumber getNumberAction = new GetNumber())
            {
                getNumberAction.SetCommandPrompt("Please enter the width");
                if (getNumberAction.Get() != GetResult.Number)
                {
                    RhinoApp.WriteLine("No width was entered.");
                    return getNumberAction.CommandResult();
                }
                width = getNumberAction.Number();
                pt0 = new Point3d(0, 0, 0);
                pt1 = new Point3d(0, width, 0);
            }

            double height;
            Point3d pt2;
            Point3d pt3;
            using (GetNumber getNumberAction = new GetNumber())
            {
                getNumberAction.SetCommandPrompt("Please enter the height");
                if (getNumberAction.Get() != GetResult.Number)
                {
                    RhinoApp.WriteLine("No height was entered.");
                    return getNumberAction.CommandResult();
                }
                height = getNumberAction.Number();
                pt2 = new Point3d(0, width, height);
                pt3 = new Point3d(0, 0, height);
            }

            doc.Objects.AddLine(pt0, pt1);
            doc.Objects.AddLine(pt1, pt2);
            doc.Objects.AddLine(pt2, pt3);
            doc.Objects.AddLine(pt3, pt0);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command created a wireframe to the window document.", EnglishName);


            return Result.Success;
        }
    }
}
