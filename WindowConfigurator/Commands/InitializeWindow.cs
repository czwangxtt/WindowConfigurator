using System;
using System.IO;
using System.Linq;
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

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            doc.Objects.Clear();

            RhinoApp.WriteLine("The {0} command will create a wireframe right now.", EnglishName);
            RhinoApp.WriteLine("Please select a json file.");
            var fd = new Rhino.UI.OpenFileDialog { Filter = "Json Files (*.json)|*.json" };
            if (!fd.ShowOpenDialog())
                return Result.Cancel;

            string fileName = fd.FileName;
            string input = File.ReadAllText(fileName);
            JsonInput deserializedInput = JsonConvert.DeserializeObject<JsonInput>(input);


            //double width;
            //Point3d pt0;
            //Point3d pt1;

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

                panelPt0 = new Point3d(5, 5, 5);
                panelPt1 = new Point3d(5, _width.CurrentValue - 5, 5);
                panelPt2 = new Point3d(5, _width.CurrentValue - 5, _height.CurrentValue - 5);
                panelPt3 = new Point3d(5, 5, _height.CurrentValue - 5);


            }

            RhinoApp.WriteLine("Command line option values are");
            RhinoApp.WriteLine(" Height = {0}", _height.CurrentValue);
            RhinoApp.WriteLine(" Width = {0}", _width.CurrentValue);

            doc.Objects.AddLine(pt0, pt1);
            doc.Objects.AddLine(pt1, pt2);
            doc.Objects.AddLine(pt2, pt3);
            doc.Objects.AddLine(pt3, pt0);

            List<Curve> panelCurves = new List<Curve>();
            panelCurves.Add(new Line(panelPt0, panelPt1).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt1, panelPt2).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt2, panelPt3).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt3, panelPt0).ToNurbsCurve());

            Curve panelContour = Curve.JoinCurves(panelCurves.ToArray())[0];
            Brep brep = Brep.CreatePlanarBreps(panelContour, doc.ModelAbsoluteTolerance)[0];
            ObjectAttributes attribute = new ObjectAttributes();
            attribute.ObjectColor = System.Drawing.Color.FromArgb(101,228,253);
            attribute.ColorSource = ObjectColorSource.ColorFromObject;
            doc.Objects.AddBrep(brep, attribute);
            

            doc.Views.Redraw();


            RhinoApp.WriteLine("The {0} command created a wireframe to the window document.", EnglishName);


            



            //using (GetNumber getNumberAction = new GetNumber())
            //{
            //    getNumberAction.SetCommandPrompt("Please enter the width");
            //    if (getNumberAction.Get() != GetResult.Number)
            //    {
            //        RhinoApp.WriteLine("No width was entered.");
            //        return getNumberAction.CommandResult();
            //    }
            //    width = getNumberAction.Number();
            //    //deserializedInput.configuration.windowWidth = width;

            //    pt0 = new Point3d(0, 0, 0);
            //    pt1 = new Point3d(0, width, 0);
            //}

            //double height;
            //Point3d pt2;
            //Point3d pt3;
            //using (GetNumber getNumberAction = new GetNumber())
            //{
            //    getNumberAction.SetCommandPrompt("Please enter the height");
            //    if (getNumberAction.Get() != GetResult.Number)
            //    {
            //        RhinoApp.WriteLine("No height was entered.");
            //        return getNumberAction.CommandResult();
            //    }
            //    height = getNumberAction.Number();
            //    //deserializedInput.configuration.windowHeight = height;

            //    pt2 = new Point3d(0, width, height);
            //    pt3 = new Point3d(0, 0, height);
            //}

            window = new Window(_height.CurrentValue, _width.CurrentValue);

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
