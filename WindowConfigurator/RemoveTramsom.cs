using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;


namespace WindowConfigurator
{
    public class RemoveTramsom : Command
    {
        static RemoveTramsom _instance;
        public RemoveTramsom()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RemoveTramsom command.</summary>
        public static RemoveTramsom Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RemoveTramsom"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}