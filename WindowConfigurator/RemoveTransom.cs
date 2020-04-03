using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;


namespace WindowConfigurator
{
    public class RemoveTransom : Command
    {
        static RemoveTransom _instance;
        public RemoveTransom()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RemoveTramsom command.</summary>
        public static RemoveTransom Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RemoveTransom"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}