using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WindowConfigurator.Input
{
    class Mapping
    {
        private Dictionary<string, object> storage = new Dictionary<string, object>();
        private JsonInput jsonInput = new JsonInput();

        public Mapping()
        {
            string fileName = @"d:\a.json";
            string input = File.ReadAllText(fileName);
            jsonInput = JsonConvert.DeserializeObject<JsonInput>(input);
        }

        public void update()
        {
            storage.Add("age", 12);
            storage.Add("name", "test");
            storage.Add("bmi", 24.1);

            int a = (int)storage["age"];
            string b = (string)storage["name"];
            double c = (double)storage["bmi"];
        }

    }
}
