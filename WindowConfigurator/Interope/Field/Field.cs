using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator.Interop;

namespace WindowConfigurator.Interop
{
    public class Field
    {
        public int id { get; protected set; }

        public Guid guid { get; set; }
        public Boolean isVisible { get; set; }
        public GlazingPanel glazingPanel { get; set; }

        public Field(double width, double height)
        {
            
        }
    }
}
