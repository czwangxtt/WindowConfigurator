using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using WindowConfigurator.Geometry;
using WindowConfigurator.Core;
using netDxf;
using netDxf.Entities;



namespace WindowConfigurator
{
    public class AddTransom : Command
    {
        static AddTransom _instance;
        public AddTransom()
        {
            _instance = this;
        }

        ///<summary>The only instance of the MyCommand1 command.</summary>
        public static AddTransom Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "AddTransom"; }
        }

        private static DxfDocument OpenProfile(string filename)
        {
            // open the profile file
            FileInfo fileInfo = new FileInfo(filename);

            // check if profile file is valid
            if (!fileInfo.Exists)
            {
                RhinoApp.WriteLine("THE FILE {0} DOES NOT EXIST", filename);
                return null;
            }
            DxfDocument dxf = DxfDocument.Load(filename, new List<string> { @".\Support" });

            // check if there has been any problems loading the file,
            if (dxf == null)
            {
                RhinoApp.WriteLine("ERROR LOADING {0}", filename);
                return null;
            }

            return dxf;
        }

        public List<Polygon> GetGeometry(string filename)
        {
            // read the dxf file
            DxfDocument dxfTest = OpenProfile(filename);
            int numberSegments = 1;
            int blockNumber = -1;

            var polygons = new List<Polygon>();

            // loop over all relevant blacks and store the hatch boundaries
            foreach (var bl in dxfTest.Blocks)
            {
                RhinoApp.WriteLine("The x location {0} ", bl.Origin.X);

                foreach (var ent in bl.Entities)
                {
                    if (ent.Layer.Name.ToString().Contains("hatch"))
                    {
                        Polygon Poly = new Polygon();
                        blockNumber++;
                        netDxf.Entities.HatchPattern hp = netDxf.Entities.HatchPattern.Solid;
                        netDxf.Entities.Hatch myHatch = new netDxf.Entities.Hatch(hp, false);
                        myHatch = (netDxf.Entities.Hatch)ent;
                        int pathNumber = -1;

                        foreach (var bPath in myHatch.BoundaryPaths)
                        {
                            pathNumber++;
                            var contour = new List<Point3d>();

                            // Store the contour
                            for (int i = 0; i < bPath.Edges.Count; i++)
                            {
                                RhinoApp.WriteLine(bPath.Edges[i].Type.ToString().ToLower());
                                switch (bPath.Edges[i].Type.ToString().ToLower())
                                {
                                    case "polyline":
                                        var myPolyline = (HatchBoundaryPath.Polyline)bPath.Edges[i];
                                        foreach (var vertex in myPolyline.Vertexes)
                                        {

                                            var vPolyline = new Point3d();
                                            vPolyline.Y = vertex.Y;
                                            if (vertex.X < 0)
                                                vPolyline.X = -vertex.X;
                                            else
                                                vPolyline.X = vertex.X;
                                            contour.Add(vPolyline);
                                        }
                                        break;

                                    case "line":

                                        var myLine = (HatchBoundaryPath.Line)bPath.Edges[i];
                                        var vLine = new Point3d();
                                        vLine.X = myLine.Start.X;
                                        vLine.Y = myLine.Start.Y;
                                        contour.Add(vLine);
                                        break;

                                    case "arc":
                                        var myArc = (HatchBoundaryPath.Arc)bPath.Edges[i];
                                        double delta = (myArc.EndAngle - myArc.StartAngle) / numberSegments;

                                        for (int j = 0; j < numberSegments; j++)
                                        {
                                            var vArc = new Point3d();
                                            double angleArc = (myArc.StartAngle + j * delta) * Math.PI / 180.0;
                                            if (myArc.IsCounterclockwise == true)
                                            {
                                                vArc.X = myArc.Center.X + myArc.Radius * Math.Cos(angleArc);
                                                vArc.Y = myArc.Center.Y + myArc.Radius * Math.Sin(angleArc);
                                            }
                                            else
                                            {
                                                vArc.X = myArc.Center.X + myArc.Radius * Math.Cos(Math.PI + angleArc);
                                                vArc.Y = myArc.Center.Y + myArc.Radius * Math.Sin(Math.PI - angleArc);
                                            }
                                            contour.Add(vArc);
                                        }
                                        break;

                                    case "ellipse":
                                        var myEllipse = (HatchBoundaryPath.Ellipse)bPath.Edges[i];
                                        double deltaEllipse = (myEllipse.EndAngle - myEllipse.StartAngle) / numberSegments;

                                        for (int j = 0; j < numberSegments; j++)
                                        {
                                            var vEllipse = new Point3d();
                                            var ellipseRadius = Math.Sqrt(Math.Pow(myEllipse.EndMajorAxis.X, 2) + Math.Pow(myEllipse.EndMajorAxis.Y, 2));

                                            double angleEllipse = (myEllipse.StartAngle + j * deltaEllipse) * Math.PI / 180.0;
                                            if (myEllipse.IsCounterclockwise == true)
                                            {
                                                vEllipse.X = myEllipse.Center.X + ellipseRadius * Math.Cos(angleEllipse);
                                                vEllipse.Y = myEllipse.Center.Y + ellipseRadius * Math.Sin(angleEllipse);
                                            }
                                            else
                                            {
                                                vEllipse.X = myEllipse.Center.X + ellipseRadius * Math.Cos(Math.PI + angleEllipse);
                                                vEllipse.Y = myEllipse.Center.Y + ellipseRadius * Math.Sin(Math.PI - angleEllipse);
                                            }
                                            contour.Add(vEllipse);
                                        }
                                        break;

                                }
                            }

                            bool isHole = true;
                            // Check if is hole
                            if (blockNumber > -1)
                            {
                                if (pathNumber == 0)
                                {
                                    isHole = false;
                                }
                                Poly.AddContour(contour, isHole);
                            }
                        }
                        polygons.Add(Poly);
                    }
                }
            }
            return polygons;
        }

        public Curve CreateCurve(List<Point3d> points)
        {
            RhinoApp.WriteLine("{0} points in current polygon loaded", points.Count);

            List<NurbsCurve> lines = new List<NurbsCurve>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                lines.Add(new Rhino.Geometry.Line(points[i], points[i + 1]).ToNurbsCurve());
            }
            lines.Add(new Rhino.Geometry.Line(points[points.Count - 1], points[0]).ToNurbsCurve());
            Curve contour = Curve.JoinCurves(lines.ToArray())[0];

            return contour;
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command will add a transom now.", EnglishName);

            double offset = 30;

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
                if (pt0.Z != pt1.Z)
                {
                    RhinoApp.WriteLine("Invalid transom.");
                    return getPointAction.CommandResult();
                }
            }

            using (GetString getArticleNumberAction = new GetString())
            {
                getArticleNumberAction.SetCommandPrompt("Please input the article Number.");
                if (getArticleNumberAction.Get() != GetResult.String)
                {
                    RhinoApp.WriteLine("No article number was input.");
                    return getArticleNumberAction.CommandResult();
                }

                //TODO AddArticleNumber to Window wireframe.
            }

            #region extrusion
            RhinoApp.WriteLine("Please select a dxf file.");
            var fileDialog = new Rhino.UI.OpenFileDialog { Filter = "DXF Files (*.dxf)|*.dxf" };
            if (!fileDialog.ShowOpenDialog())
                return Result.Cancel;

            string filename = fileDialog.FileName;
            List<Polygon> geometry = GetGeometry(filename);
            RhinoApp.WriteLine("{0} polygons loaded", geometry.Count);

            List<Point3d> extrusionVertex = new List<Point3d>();
            extrusionVertex.Add(new Point3d(0, 1500, 0));
            extrusionVertex.Add(new Point3d(0, 0, 0));
            extrusionVertex.Add(new Point3d(0, 0, 1000));
            extrusionVertex.Add(new Point3d(0, 1500, 1000));
            Curve rail_crv = CreateCurve(extrusionVertex);
            rail_crv.Domain = new Interval(0, 4000);
            //doc.Objects.AddCurve(rail_crv);

            Polygon polygon = geometry[0];
            List<Point3d> points = polygon.outCountour;
            RhinoApp.WriteLine("{0} points in current polygon loaded", points.Count);
            if (points.Count < 1)
            {
                RhinoApp.WriteLine("Error: polygon must contain at least one point");
            }
            Curve cross_sections = CreateCurve(polygon.outCountour);

            //doc.Objects.AddCurve(cross_sections);

            var breps = Brep.CreateFromSweep(rail_crv, cross_sections, true, doc.ModelAbsoluteTolerance);
            RhinoApp.WriteLine("Brep numbers: {0} ", breps.Length);

            ObjectAttributes framePaintAttribute = new ObjectAttributes();
            framePaintAttribute.ObjectColor = System.Drawing.Color.FromArgb(58, 69, 77);
            framePaintAttribute.ColorSource = ObjectColorSource.ColorFromObject;
            for (int i = 0; i < breps.Length; i++)
                doc.Objects.AddBrep(breps[i], framePaintAttribute);
            #endregion

            Guid guid = doc.Objects.AddLine(pt0, pt1);
            Guid extrusionGuid = doc.Objects.AddLine(pt0, pt1);

            Point3 p0;
            Point3 p1;
            if (pt0.Y < pt1.Y)
            {
                p0 = new Point3(pt0.X, pt0.Y, pt0.Z);
                p1 = new Point3(pt1.X, pt1.Y, pt1.Z);
            }
            else
            {
                p0 = new Point3(pt1.X, pt1.Y, pt1.Z);
                p1 = new Point3(pt0.X, pt0.Y, pt0.Z);
            }

            #region Wireframe
            Transom transom = new Transom(p0, p1, guid, extrusionGuid);
            InitializeWindow.window.wireFrame.addIntermediate(transom);

            ObjectAttributes trimAttribute = new ObjectAttributes();
            trimAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 0, 0);
            trimAttribute.ColorSource = ObjectColorSource.ColorFromObject;

            Guid trimGuid0 = doc.Objects.AddLine(new Point3d(pt0.X, pt0.Y + offset, pt0.Z + offset), new Point3d(pt0.X, pt0.Y + offset, pt0.Z - offset), trimAttribute);
            Guid trimGuid1 = doc.Objects.AddLine(new Point3d(pt1.X, pt1.Y - offset, pt1.Z + offset), new Point3d(pt1.X, pt1.Y - offset, pt1.Z - offset), trimAttribute);
            InitializeWindow.window.wireFrame._frames.Last().Connects[0].guid = trimGuid0;
            InitializeWindow.window.wireFrame._frames.Last().Connects[1].guid = trimGuid1;
            #endregion

            #region Field
            Field originalFrame = InitializeWindow.window.wireFrame._fields[0];

           
            ObjectAttributes glazingAttribute = new ObjectAttributes();
            glazingAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 255, 255);
            glazingAttribute.ColorSource = ObjectColorSource.ColorFromObject;
            

            Point3d panel1Pt0 = new Point3d(11, offset, offset);
            Point3d panel1Pt1 = new Point3d(11, transom.endPoint.Y - offset, offset);
            Point3d panel1Pt2 = new Point3d(11, transom.endPoint.Y - offset, transom.endPoint.Z - 9);
            Point3d panel1Pt3 = new Point3d(11, transom.startPoint.Y + offset, transom.startPoint.Z - 9);

            List<Curve> panel1Curves = new List<Curve>();
            panel1Curves.Add(new Rhino.Geometry.Line(panel1Pt0, panel1Pt1).ToNurbsCurve());
            panel1Curves.Add(new Rhino.Geometry.Line(panel1Pt1, panel1Pt2).ToNurbsCurve());
            panel1Curves.Add(new Rhino.Geometry.Line(panel1Pt2, panel1Pt3).ToNurbsCurve());
            panel1Curves.Add(new Rhino.Geometry.Line(panel1Pt3, panel1Pt0).ToNurbsCurve());

            Curve panel1Contour = Curve.JoinCurves(panel1Curves.ToArray())[0];
            Brep brep1 = Brep.CreatePlanarBreps(panel1Contour, doc.ModelAbsoluteTolerance)[0];
            doc.Objects.AddBrep(brep1, glazingAttribute);

            Point3d panel2Pt0 = new Point3d(15, transom.startPoint.Y + offset, transom.startPoint.Z + 9);
            Point3d panel2Pt1 = new Point3d(15, transom.endPoint.Y - offset, transom.endPoint.Z + 9);
            Point3d panel2Pt2 = new Point3d(15, transom.endPoint.Y - offset, 1000 - offset);
            Point3d panel2Pt3 = new Point3d(15, offset, 1000 - offset);

            List<Curve> panel2Curves = new List<Curve>();
            panel2Curves.Add(new Rhino.Geometry.Line(panel2Pt0, panel2Pt1).ToNurbsCurve());
            panel2Curves.Add(new Rhino.Geometry.Line(panel2Pt1, panel2Pt2).ToNurbsCurve());
            panel2Curves.Add(new Rhino.Geometry.Line(panel2Pt2, panel2Pt3).ToNurbsCurve());
            panel2Curves.Add(new Rhino.Geometry.Line(panel2Pt3, panel2Pt0).ToNurbsCurve());

            Curve panel2Contour = Curve.JoinCurves(panel2Curves.ToArray())[0];
            Brep brep2 = Brep.CreatePlanarBreps(panel2Contour, doc.ModelAbsoluteTolerance)[0];
            doc.Objects.AddBrep(brep2, glazingAttribute);
            
            doc.Objects.Delete(originalFrame.guid, true);
            #endregion


            doc.Views.Redraw();

            RhinoApp.WriteLine("The {0} command added one transom to the document.", EnglishName);

            return Result.Success;
        }
    }
}