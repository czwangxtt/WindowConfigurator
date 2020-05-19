using System;
using Rhino;
using Rhino.Commands;

namespace WindowConfigurator.Commands
{
    public class SwitchSystem : Command
    {
        static SwitchSystem _instance;
        public SwitchSystem()
        {
            _instance = this;
        }

        ///<summary>The only instance of the SwitchSystem command.</summary>
        public static SwitchSystem Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "SwitchSystem"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}