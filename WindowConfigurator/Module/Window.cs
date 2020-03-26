using System;
using System.Collections.Generic;
using System.Text;
using WindowConfiguratorCommnon;
using WindowConfiguratorCommnon.Input;
using WindowConfiguratorCommnon.Interop;

namespace WindowConfiguratorCommnon.Module
{
    class Window
    {
        public static double width { get; set; }
        public static double height { get; set; }

        public static WireFrame wireFrame { get; set; }
        public static Field field { get; set; }


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
