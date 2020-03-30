using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator;
using WindowConfigurator.Input;
using WindowConfigurator.Interop;
using Newtonsoft.Json;

namespace WindowConfigurator.Module
{
    class Window
    {
        [JsonProperty]
        public static double width { get; set; }

        [JsonProperty]
        public static double height { get; set; }

        [JsonProperty]
        public static WireFrame wireFrame { get; set; }

        [JsonProperty]
        public static Field field { get; set; }


        /// <summary>
        /// Initializes a window system with input object.
        /// </summary>
        /// <param name="input">Converted json input</param>
        public Window(WindowInput input)
        {
            width = input.configuration.windowWidth;
            height = input.configuration.windowHeight;
            
            wireFrame = new WireFrame(width, height);
            field = new Field(width, height);
        }
    }
}
