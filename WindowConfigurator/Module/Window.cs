using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator;
using WindowConfigurator.Input;
using WindowConfigurator.Interop;

namespace WindowConfigurator.Module
{
    class Window
    {
        public double width { get; set; }
        public double height { get; set; }

        public WireFrame wireFrame { get; set; }
        public Field field { get; set; }


        /// <summary>
        /// Initializes a window system with input object.
        /// </summary>
        /// <param name="input">Converted json input</param>
        public Window(WindowInput input)
        {
            width = 1500;
            height = 3000;
            
            wireFrame = new WireFrame(width, height);
            field = new Field(width, height);
        }
    }
}
