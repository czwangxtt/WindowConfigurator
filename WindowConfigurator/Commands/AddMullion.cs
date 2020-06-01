using System;
using System.Linq;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using WindowConfigurator.Geometry;
using WindowConfigurator.Core;
using System.Collections.Generic;

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

            double offset = 18.0;

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

            using (GetString getArticleNumberAction = new GetString())
            {
                getArticleNumberAction.SetCommandPrompt("Please input the article Number");
                if (getArticleNumberAction.Get() != GetResult.String)
                {
                    RhinoApp.WriteLine("No article number was input.");
                    return getArticleNumberAction.CommandResult();
                }

                //TODO AddArticleNumber to Window wireframe.
            }

            Guid guid = doc.Objects.AddLine(pt0, pt1);

            Point3 p0;
            Point3 p1;
            if (pt0.Z < pt1.Z)
            {
                p0 = new Point3(pt0.X, pt0.Y, pt0.Z);
                p1 = new Point3(pt1.X, pt1.Y, pt1.Z);
            }
            else
            {
                p0 = new Point3(pt1.X, pt1.Y, pt1.Z);
                p1 = new Point3(pt0.X, pt0.Y, pt0.Z);
            }

            Mullion mullion = new Mullion(p0, p1, guid);
            InitializeWindow.window.wireFrame.addIntermediate(mullion);

            ObjectAttributes trimAttribute = new ObjectAttributes();
            trimAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 0, 0);
            trimAttribute.ColorSource = ObjectColorSource.ColorFromObject;

            Guid trimGuid0 = doc.Objects.AddLine(new Point3d(pt0.X, pt0.Y - offset, pt0.Z + offset), new Point3d(pt0.X, pt0.Y + offset, pt0.Z + offset), trimAttribute);
            Guid trimGuid1 = doc.Objects.AddLine(new Point3d(pt1.X, pt1.Y - offset, pt1.Z - offset), new Point3d(pt1.X, pt1.Y + offset, pt1.Z - offset), trimAttribute);
            InitializeWindow.window.wireFrame._frames.Last().Connects[0].guid = trimGuid0;
            InitializeWindow.window.wireFrame._frames.Last().Connects[1].guid = trimGuid1;

            #region Field
            Field originalFrame = InitializeWindow.window.wireFrame._fields[0];


            ObjectAttributes glazingAttribute = new ObjectAttributes();
            glazingAttribute.ObjectColor = System.Drawing.Color.FromArgb(101, 228, 253);
            glazingAttribute.ColorSource = ObjectColorSource.ColorFromObject;


            Point3d panel1Pt0 = new Point3d(0, offset, offset);
            Point3d panel1Pt1 = new Point3d(0, mullion.startPoint.Y - 4, offset);
            Point3d panel1Pt2 = new Point3d(0, mullion.endPoint.Y - 4, mullion.endPoint.Z - offset);
            Point3d panel1Pt3 = new Point3d(0, offset, mullion.endPoint.Z - offset);

            List<Curve> panel1Curves = new List<Curve>();
            panel1Curves.Add(new Line(panel1Pt0, panel1Pt1).ToNurbsCurve());
            panel1Curves.Add(new Line(panel1Pt1, panel1Pt2).ToNurbsCurve());
            panel1Curves.Add(new Line(panel1Pt2, panel1Pt3).ToNurbsCurve());
            panel1Curves.Add(new Line(panel1Pt3, panel1Pt0).ToNurbsCurve());

            Curve panel1Contour = Curve.JoinCurves(panel1Curves.ToArray())[0];
            Brep brep1 = Brep.CreatePlanarBreps(panel1Contour, doc.ModelAbsoluteTolerance)[0];
            doc.Objects.AddBrep(brep1, glazingAttribute);

            Point3d panel2Pt0 = new Point3d(0, mullion.startPoint.Y + 9, mullion.startPoint.Z + offset);
            Point3d panel2Pt1 = new Point3d(0, 1500 - offset, offset);
            Point3d panel2Pt2 = new Point3d(0, 1500 - offset, 1000 - offset);
            Point3d panel2Pt3 = new Point3d(0, mullion.startPoint.Y + 9, 1000 - offset);

            List<Curve> panel2Curves = new List<Curve>();
            panel2Curves.Add(new Line(panel2Pt0, panel2Pt1).ToNurbsCurve());
            panel2Curves.Add(new Line(panel2Pt1, panel2Pt2).ToNurbsCurve());
            panel2Curves.Add(new Line(panel2Pt2, panel2Pt3).ToNurbsCurve());
            panel2Curves.Add(new Line(panel2Pt3, panel2Pt0).ToNurbsCurve());

            Curve panel2Contour = Curve.JoinCurves(panel2Curves.ToArray())[0];
            Brep brep2 = Brep.CreatePlanarBreps(panel2Contour, doc.ModelAbsoluteTolerance)[0];
            doc.Objects.AddBrep(brep2, glazingAttribute);

            doc.Objects.Delete(originalFrame.guid, true);
            #endregion

            doc.Views.Redraw();

            RhinoApp.WriteLine("The {0} command added one mullion to the document.", EnglishName);

            return Result.Success;
        }
    }
}