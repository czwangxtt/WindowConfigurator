using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WindowConfigurator.Input
{
    class Maping
    {
        private Dictionary<string, object> storage = new Dictionary<string, object>();
        private JsonInput jsonInput = new JsonInput();

        public Maping()
        {
            string fileName = @"d:\a.json";
            string input = File.ReadAllText(fileName);
            jsonInput = JsonConvert.DeserializeObject<JsonInput>(input);
        }
        
    }
}
