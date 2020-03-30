using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;


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
            using (GetObject getObjectAction = new GetObject())
            {
                getObjectAction.SetCommandPrompt("Please select a mullion.");
                if (getObjectAction.Get() != GetResult.Object)
                {
                    RhinoApp.WriteLine("No mullion was selected.");
                    return getObjectAction.CommandResult();
                }
                doc.Objects.Delete(getObjectAction.Object(0), true, true);

                //TODO AddArticleNumber to Window wireframe.
            }

            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command removed one mullion to the document.", EnglishName);

            return Result.Success;
        }
    }
}