using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WindowConfiguratorCommnon.Input
{
    class WindowFrameConfiguration
    {
        public string length { get; set; }
        public string headProfile { get; set; }
    }

    //class HeadProfile : WindowFrameConfiguration
    //{
    //    public string headProfile { get; set; }
    //}

    //class SillProfile : WindowFrameConfiguration
    //{
    //    public string sillProfile { get; set; }
    //}

    //class RightJambProfile : WindowFrameConfiguration
    //{
    //    public string rightJamb { get; set; }
    //}

    //class LeftJambProfile : WindowFrameConfiguration
    //{
    //    public string leftJamb { get; set; }
    //}


    class IntermediateConfiguration
    {
        public string intermediateProfile { get; set; }
        public string intermediateDirection { get; set; }
        public string locator { get; set; }
    }

    class ConnectionConfiguration
    {
        public string profileEndA { get; set; }
        public string profileEndB { get; set; }
    }

    class OperabilityConfiguration
    {
        public string operabilityType { get; set; }
        public string fieldA { get; set; }
        public string fieldB { get; set; }
        public string openingDirection { get; set; }
        public string ventProfile { get; set; }
        public string insertProfile { get; set; }
        public string reverseRebateProfile { get; set; }
    }

    class GlassConfiguration
    {
        public string fieldName { get; set; }
        public string glazingBeadArticleNumber { get; set; }
        public string steppedGlass { get; set; }
    }

    class JsonInput
    {
        public string systemType { get; set; }

        [JsonProperty("windowFrameConfiguration")]
        public List<WindowFrameConfiguration> windowFrameConfigurations { get; set; }

        [JsonProperty("intermediateConfigurations")]
        public List<IntermediateConfiguration> intermediateConfigurations { get; set; }

        [JsonProperty("connectionConfiguration")]
        public List<ConnectionConfiguration> connectionConfigurations { get; set; }

        [JsonProperty("operabilityConfiguration")]
        public List<OperabilityConfiguration> operabilityConfigurations { get; set; }

        [JsonProperty("glassConfiguration")]
        public List<GlassConfiguration> glassConfigurations { get; set; }
    }
}
