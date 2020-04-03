using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WindowConfigurator.Input
{
    public class Mapping
    {
        // Mapping dictionary with different type of value.
        private Dictionary<string, double> doubleMapping = new Dictionary<string, double>();
        private Dictionary<string, int> intMapping = new Dictionary<string, int>();
        private Dictionary<string, string> stringMapping = new Dictionary<string, string>();
        private Dictionary<string, Boolean> booleanMapping = new Dictionary<string, Boolean>();


        /// <summary>
        /// Check if the Mapping dictionary contains a variable.
        /// </summary>
        /// <param name="variableName">The name of variable</param>
        /// <returns></returns>
        public Boolean ContainsKey(string variableName)
        {
            if (intMapping.ContainsKey(variableName))
                return true;

            if (doubleMapping.ContainsKey(variableName))
                return true;

            if (stringMapping.ContainsKey(variableName))
                return true;

            if (stringMapping.ContainsKey(variableName))
                return true;

            return false;
        }


        /// <summary>
        /// Add a key to the Mapping dictionary.
        /// </summary>
        /// <param name="variableName">The name of variable</param>
        /// <param name="type">The type of variable</param>
        public void AddKey(string variableName, string type)
        {
            switch (type.ToUpper())
            {
                case "INT":
                    if (!intMapping.ContainsKey(variableName))
                        intMapping[variableName] = 0;
                    break;

                case "DOUBLE":
                    if (!doubleMapping.ContainsKey(variableName))
                        doubleMapping[variableName] = 0.0;
                    break;

                case "STRING":
                    if (!stringMapping.ContainsKey(variableName))
                        stringMapping[variableName] = "";
                    break;

                case "BOOLEAN":
                    if (!booleanMapping.ContainsKey(variableName))
                        booleanMapping[variableName] = false;
                    break;
            }
        }

        public void update()
        {

        }

        

        public int getIntValueByKey(string variableNanme)
        {
            return intMapping[variableNanme];
        }

        public double getDoubleValueByKey(string variableNanme)
        {
            return doubleMapping[variableNanme];
        }

        public string getStringValueByKey(string variableNanme)
        {
            return stringMapping[variableNanme];
        }

        public Boolean getBooleanValueByKey(string variableNanme)
        {
            return booleanMapping[variableNanme];
        }

    }
}
