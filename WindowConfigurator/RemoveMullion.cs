using System;
using Rhino;
using Rhino.Commands;

namespace WindowConfigurator
{
    public class RemoveMullion : Command
    {
        static RemoveMullion _instance;
        public RemoveMullion()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RemoveMullion command.</summary>
        public static RemoveMullion Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RemoveMullion"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}