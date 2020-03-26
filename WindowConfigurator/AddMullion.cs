using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace WindowConfigurator
{
    public class AddMullion : Command
    {
        static AddMullion _instance;
        public AddMullion()
        {
            _instance = this;
        }

        ///<summary>The only instance of the AddMullion command.</summary>
        public static AddMullion Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "AddMullion"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will add a mullion now.", EnglishName);

            Point3d pt0;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the start point");
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No start point was selected.");
                    return getPointAction.CommandResult();
                }
                pt0 = getPointAction.Point();
            }

            Point3d pt1;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DynamicDraw +=
                  (sender, e) => e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.DarkRed);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return getPointAction.CommandResult();
                }
                pt1 = getPointAction.Point();
                if (pt0.Y != pt1.Y)
                {
                    RhinoApp.WriteLine("Invalid Mullion.");
                    return getPointAction.CommandResult();
                }
            }


            doc.Objects.AddLine(pt0, pt1);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one mullion to the document.", EnglishName);

            return Result.Success;
        }
    }
}