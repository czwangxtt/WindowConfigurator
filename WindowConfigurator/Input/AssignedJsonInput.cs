//using System;
//using System.Collections.Generic;
//using System.Text;
//using Newtonsoft.Json;

//namespace WindowConfigurator.Input
//{
//    class WindowFrameConfiguration
//    {
//        public string profile { get; set; }
//        public string length { get; set; }
//    }


//    class IntermediateConfiguration
//    {
//        public string intermediateProfile { get; set; }
//        public string intermediateDirection { get; set; }
//        public string locator { get; set; }
//    }

//    class ConnectionConfiguration
//    {
//        public string connectType { get; set; }
//        public string position { get; set; }
//        public string connectFrameId { get; set; }
//    }

//    class OperabilityConfiguration
//    {
//        public string operabilityType { get; set; }
//        public string fieldA { get; set; }
//        public string fieldB { get; set; }
//        public string openingDirection { get; set; }
//        public string ventProfile { get; set; }
//        public string insertProfile { get; set; }
//        public string reverseRebateProfile { get; set; }
//    }

//    class GlassConfiguration
//    {
//        public string fieldName { get; set; }
//        public string glazingBeadArticleNumber { get; set; }
//        public string steppedGlass { get; set; }
//    }

//    class JsonInput
//    {
//        public string systemType { get; set; }

//        [JsonProperty("windowFrameConfigurations")]
//        public List<WindowFrameConfiguration> windowFrameConfigurations { get; set; }

//        [JsonProperty("intermediateConfigurations")]
//        public List<IntermediateConfiguration> intermediateConfigurations { get; set; }

//        [JsonProperty("connectionConfigurations")]
//        public List<ConnectionConfiguration> connectionConfigurations { get; set; }

//        [JsonProperty("operabilityConfigurations")]
//        public List<OperabilityConfiguration> operabilityConfigurations { get; set; }

//        [JsonProperty("glassConfigurations")]
//        public List<GlassConfiguration> glassConfigurations { get; set; }
//    }
//}
