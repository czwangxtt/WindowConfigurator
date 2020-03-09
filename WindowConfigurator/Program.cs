using System;
using System.IO;
using System.Linq;
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
        

        static void Main(string[] args)
        {



            string fileName = @"d:\a.json";
            string input = File.ReadAllText(fileName);
            WindowInput deserializedInput = JsonConvert.DeserializeObject<WindowInput>(input);

            Window window = new Window(deserializedInput);

            window.wireFrame.addIntermediate(new Transom(new Point(0, 0, 1000), new Point(0, 1500, 1000)));
            window.wireFrame.addIntermediate(new Transom(new Point(0, 0, 2000), new Point(0, 1500, 2000)));
            //window.wireFrame.addIntermediate(new Mullion(new Point(0, 750, 1000), new Point(0, 750, 3000)));

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

            

            //SortedList<double, int> sl = new SortedList<double, int>();
            //sl.Add(0.0, 1);
            //sl.Add(10.0, 2);
            //sl.Add(3.0, 3);
            //sl.Add(4.0, 4);
            //sl.Add(2.0, 5);

            //for (int i = 0; i < sl.Count; i++)
            //{
            //    Console.WriteLine("\t{0}:\t{1}", sl.Keys[i], sl[sl.Keys[i]]);
            //}



            //void graph(Point center, int length)
            //{
            //    for (int i = 0; i < center.y; i++)
            //        Console.WriteLine(" ");
            //    for (int i = 0; i < length; i++)
            //    {
            //        string line = " ";
            //        for (int j = 0; j < center.x * 2 + length * 2 - 1; j++)
            //        {
            //            if (i == 0 || i == length - 1)
            //            {
            //                if (j >= center.x * 2 && (j - center.x * 2) % 2 == 0)
            //                    line += "*";
            //                else line += " ";
            //            }
            //            else
            //            {
            //                if (j == center.x * 2 || j == center.x * 2 + length * 2 - 2)
            //                    line += "*";
            //                else line += " ";
            //            }
            //        }
            //        Console.WriteLine(line);
            //    }
            //}

            //WireFrame wireFrame = new WireFrame(10.0, 10.0);





            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
            //Console.WriteLine(deserializedProduct.Price);
        }
    }
}
