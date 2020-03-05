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



        public Window(WindowInput input)
        {
            width = input.configuration.windowWidth;
            height = input.configuration.windowHeight;
            
            wireFrame = new WireFrame(width, height);
            field = new Field(width, height);
        }
    }
}
