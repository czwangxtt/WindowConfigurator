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
            double offset = 18.0; 

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

            panelPt0 = new Point3d(0, offset, offset);
            panelPt1 = new Point3d(0, _width.CurrentValue - offset, offset);
            panelPt2 = new Point3d(0, _width.CurrentValue - offset, _height.CurrentValue - offset);
            panelPt3 = new Point3d(0, offset, _height.CurrentValue - offset);

            Guid frameGuid0 = doc.Objects.AddLine(pt0, pt1);
            Guid frameGuid1 = doc.Objects.AddLine(pt1, pt2);
            Guid frameGuid2 = doc.Objects.AddLine(pt2, pt3);
            Guid frameGuid3 = doc.Objects.AddLine(pt3, pt0);

            List<Curve> panelCurves = new List<Curve>();
            panelCurves.Add(new Line(panelPt0, panelPt1).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt1, panelPt2).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt2, panelPt3).ToNurbsCurve());
            panelCurves.Add(new Line(panelPt3, panelPt0).ToNurbsCurve());

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


            doc.Views.Redraw();

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
