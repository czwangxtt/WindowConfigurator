using System;
using System.Collections.Generic;
using System.Text;
using WindowConfigurator;
using WindowConfigurator.Geometry;


namespace WindowConfigurator.Input
{

    public class Configuration
    {
        public string systemName { get; set; }
        public double windowHeight { get; set; }
        public double windowWidth { get; set; }
        public Boolean vertical { get; set; }
        public Boolean Horizontal { get; set; }
    }

    public class WindowFrameProfiles
    {
        public int headProfile { get; set; }
        public int sillProfile { get; set; }
        public int leftJambProfile { get; set; }
        public int rightJambProfile { get; set; }
        public List<string> intermediateProfile { get; set; }

    }

    //TODO: Remove redundant class
    //TODO: Add input class to handle intermediate

    class WindowInput
    {
        public Configuration configuration { get; set; }
        public WindowFrameProfiles windowFrameProfiles { get; set; }
    }
}
