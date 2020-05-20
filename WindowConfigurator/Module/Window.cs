using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator;
using WindowConfigurator.Input;
using WindowConfigurator.Core;
using Newtonsoft.Json;

namespace WindowConfigurator.Module
{
    public class Window
    {
        [JsonProperty]
        public double width { get; set; }

        [JsonProperty]
        public double height { get; set; }

        [JsonProperty]
        public WireFrame wireFrame { get; set; }

        [JsonProperty]
        public Field field { get; set; }


        /// <summary>
        /// Initializes a window system with input object.
        /// </summary>
        /// <param name="input">Converted json input</param>
        public Window(WindowInput input)
        {
            width = input.configuration.windowWidth;
            height = input.configuration.windowHeight;
            
            wireFrame = new WireFrame(width, height);
        }

        public Window(double width, double height)
        {
            wireFrame = new WireFrame(width, height);
        }
    }
}
