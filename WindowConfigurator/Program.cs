using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WindowConfigurator.Geometry;
using WindowConfigurator.Input;
using WindowConfigurator.Interop;
using WindowConfigurator.Module;
using WindowConfigurator.Utilities;

namespace WindowConfigurator
{
    class Program
    {
        static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            string fileName = @"d:\a.json";
            string input = File.ReadAllText(fileName);
            //Console.WriteLine(input);
            WindowInput deserializedInput = JsonConvert.DeserializeObject<WindowInput>(input);

            Window window = new Window(deserializedInput);

            Transom t1 = new Transom(new Point3(0, 0, 1000), new Point3(0, 1500, 1000));
            Transom t2 = new Transom(new Point3(0, 0, 2000), new Point3(0, 1500, 2000));
            Mullion m1 = new Mullion(new Point3(0, 750, 1000), new Point3(0, 750, 2000));
            window.wireFrame.addIntermediate(t1);
            window.wireFrame.addIntermediate(t2);
            window.wireFrame.addIntermediate(m1);
            window.wireFrame.removeIntermediate(t1);
            window.wireFrame.removeIntermediate(t2);

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (StreamWriter sw = new StreamWriter(@"d:\c.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, window);
            }

            string output = JsonConvert.SerializeObject(window);
            Console.WriteLine(output);

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
        }
    }
}
