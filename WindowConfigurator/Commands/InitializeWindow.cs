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
using netDxf;
using netDxf.Entities;
using WindowConfigurator.Input;
using WindowConfigurator.Module;

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

        public static Window window { get; set; }

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
            doc.Objects.Clear();
            RhinoApp.WriteLine("The {0} command will create a wireframe right now.", EnglishName);

            #region jsonImport
            RhinoApp.WriteLine("Please select a json file.");
            var fd = new Rhino.UI.OpenFileDialog { Filter = "Json Files (*.json)|*.json" };
            if (!fd.ShowOpenDialog())
                return Result.Cancel;

            string fileName = fd.FileName;
            string input = File.ReadAllText(fileName);
            JsonInput deserializedInput = JsonConvert.DeserializeObject<JsonInput>(input);
            #endregion

            #region dimensionInput
            //double width;
            //Point3d pt0;
            //Point3d pt1;
            double offset = 30; 

            Point3d pt0;
            Point3d pt1;
            Point3d pt2;
            Point3d pt3;

            Point3d panelPt0;
            Point3d panelPt1;
            Point3d panelPt2;
            Point3d panelPt3;

            OptionDouble _height = new OptionDouble(0, 100, 1500);
            OptionDouble _width = new OptionDouble(0, 100, 2000);

            using (GetNumber gn = new GetNumber())
            {
                gn.AddOptionDouble("Height", ref _height);
                gn.AddOptionDouble("Width", ref _width);

                GetResult get_rc = gn.Get();
                if (gn.CommandResult() != Result.Success)
                    return gn.CommandResult();

                pt0 = new Point3d(0, 0, 0);
                pt1 = new Point3d(0, _width.CurrentValue, 0);
                pt2 = new Point3d(0, _width.CurrentValue, _height.CurrentValue);
                pt3 = new Point3d(0, 0, _height.CurrentValue);

            }

            using (GetNumber gn = new GetNumber())
            {
                gn.AddOptionDouble("Height", ref _height);
                gn.AddOptionDouble("Width", ref _width);

                GetResult get_rc = gn.Get();
                if (gn.CommandResult() != Result.Success)
                    return gn.CommandResult();

                RhinoApp.WriteLine("Command line option values are");
                RhinoApp.WriteLine(" Height = {0}", _height.CurrentValue);
                RhinoApp.WriteLine(" Width = {0}", _width.CurrentValue);

                pt0 = new Point3d(0, 0, 0);
                pt1 = new Point3d(0, _width.CurrentValue, 0);
                pt2 = new Point3d(0, _width.CurrentValue, _height.CurrentValue);
                pt3 = new Point3d(0, 0, _height.CurrentValue);
            }

            RhinoApp.WriteLine("Command line option values are");
            RhinoApp.WriteLine(" Height = {0}", _height.CurrentValue);
            RhinoApp.WriteLine(" Width = {0}", _width.CurrentValue);

            panelPt0 = new Point3d(11, offset, offset);
            panelPt1 = new Point3d(11, _width.CurrentValue - offset, offset);
            panelPt2 = new Point3d(11, _width.CurrentValue - offset, _height.CurrentValue - offset);
            panelPt3 = new Point3d(11, offset, _height.CurrentValue - offset);

            ObjectAttributes frameAttribute = new ObjectAttributes();
            frameAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 0, 0);
            frameAttribute.ColorSource = ObjectColorSource.ColorFromObject;
            Guid frameGuid0 = doc.Objects.AddLine(pt0, pt1, frameAttribute);
            Guid frameGuid1 = doc.Objects.AddLine(pt1, pt2, frameAttribute);
            Guid frameGuid2 = doc.Objects.AddLine(pt2, pt3, frameAttribute);
            Guid frameGuid3 = doc.Objects.AddLine(pt3, pt0, frameAttribute);

            List<Curve> panelCurves = new List<Curve>();
            panelCurves.Add(new Rhino.Geometry.Line(panelPt0, panelPt1).ToNurbsCurve());
            panelCurves.Add(new Rhino.Geometry.Line(panelPt1, panelPt2).ToNurbsCurve());
            panelCurves.Add(new Rhino.Geometry.Line(panelPt2, panelPt3).ToNurbsCurve());
            panelCurves.Add(new Rhino.Geometry.Line(panelPt3, panelPt0).ToNurbsCurve());

            Curve panelContour = Curve.JoinCurves(panelCurves.ToArray())[0];
            Brep brep = Brep.CreatePlanarBreps(panelContour, doc.ModelAbsoluteTolerance)[0];
            ObjectAttributes glazingAttribute = new ObjectAttributes();
            glazingAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 255, 255);
            glazingAttribute.ColorSource = ObjectColorSource.ColorFromObject;
            Guid initialFieldGuid = doc.Objects.AddBrep(brep, glazingAttribute);
            

            ObjectAttributes trimAttribute = new ObjectAttributes();
            trimAttribute.ObjectColor = System.Drawing.Color.FromArgb(255, 0, 0);
            trimAttribute.ColorSource = ObjectColorSource.ColorFromObject;

            Guid trimGuid0 = doc.Objects.AddLine(new Point3d(0, pt0.Y + offset, pt0.Z + offset), pt0, trimAttribute);
            Guid trimGuid1 = doc.Objects.AddLine(new Point3d(0, pt1.Y - offset, pt1.Z + offset), pt1, trimAttribute);
            Guid trimGuid2 = doc.Objects.AddLine(new Point3d(0, pt2.Y - offset, pt2.Z - offset), pt2, trimAttribute);
            Guid trimGuid3 = doc.Objects.AddLine(new Point3d(0, pt3.Y + offset, pt3.Z - offset), pt3, trimAttribute);
            #endregion


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


            doc.Views.Redraw();


            RhinoApp.WriteLine("Sweep success");
            RhinoApp.WriteLine("The {0} command created a wireframe to the window document.", EnglishName);


            window = new Window(_width.CurrentValue, _height.CurrentValue);

            window.wireFrame._fields.Add(new Core.Field(initialFieldGuid));

            window.wireFrame._frames[1].guid = frameGuid0;
            window.wireFrame._frames[3].guid = frameGuid1;
            window.wireFrame._frames[0].guid = frameGuid2;
            window.wireFrame._frames[2].guid = frameGuid3;

            window.wireFrame._frames[1].Connects[0].guid = trimGuid0;
            window.wireFrame._frames[3].Connects[0].guid = trimGuid1;
            window.wireFrame._frames[0].Connects[0].guid = trimGuid2;
            window.wireFrame._frames[2].Connects[0].guid = trimGuid3;

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(@"D:\GitHub\WindowConfigurator\WindowConfigurator\Output\output.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, window);
            }

            string output = JsonConvert.SerializeObject(window);
            Console.WriteLine(output);

            return Result.Success;
        }
    }
}
