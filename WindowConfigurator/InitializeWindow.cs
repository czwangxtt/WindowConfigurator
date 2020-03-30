using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
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

            RhinoApp.WriteLine("Please select a json file.");
            var fd = new Rhino.UI.OpenFileDialog { Filter = "Json Files (*.json)|*.json" };
            if (!fd.ShowOpenDialog())
                return Result.Cancel;

            string fileName = fd.FileName;
            string input = File.ReadAllText(fileName);
            WindowInput deserializedInput = JsonConvert.DeserializeObject<WindowInput>(input);


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
                deserializedInput.configuration.windowWidth = width;

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
                deserializedInput.configuration.windowHeight = height;

                pt2 = new Point3d(0, width, height);
                pt3 = new Point3d(0, 0, height);
            }

            

            
            

            Window window = new Window(deserializedInput);
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(@"d:\c.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, window);
            }

            string output = JsonConvert.SerializeObject(window);
            Console.WriteLine(output);

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
