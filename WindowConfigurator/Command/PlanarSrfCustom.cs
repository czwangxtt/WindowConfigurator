using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using Newtonsoft.Json;
using WindowConfigurator.Input;
using WindowConfigurator.Module;

namespace WindowConfigurator
{
    public class PlanarSrfCustom : Command
    {
        static PlanarSrfCustom _instance;
        public PlanarSrfCustom()
        {
            _instance = this;
        }

        ///<summary>The only instance of the PlanarSrfCustom command.</summary>
        public static PlanarSrfCustom Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "PlanarSrfCustom"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            using (GetObject getCurvesAction = new GetObject())
            {
                getCurvesAction.SetCommandPrompt("Please select two curves");
                getCurvesAction.GroupSelect = true;
                getCurvesAction.GetMultiple(1, 2);
                getCurvesAction.GeometryFilter = ObjectType.Curve;
                getCurvesAction.GeometryAttributeFilter = GeometryAttributeFilter.ClosedCurve;
                getCurvesAction.DisablePreSelect();
                getCurvesAction.SubObjectSelect = false;
                RhinoApp.WriteLine("Two curves selected");

                Brep[] planar = Brep.CreatePlanarBreps(getCurvesAction.Object(0).Curve(), doc.ModelAbsoluteTolerance);

                Vector3d extrusionDirect = new Vector3d(0, 0, 0.25);
                Surface extrusion = Surface.CreateExtrusion(getCurvesAction.Object(1).Curve(), extrusionDirect);


                foreach (var p in planar)
                {
                    Brep[] newp = p.Split(extrusion.ToBrep(), 0.25);
                    doc.Objects.AddBrep(newp[0]);
                }
                doc.Views.Redraw();
            }

            RhinoApp.WriteLine("The {0} command created a planar surface to the document.", EnglishName);
            return Result.Success;
        }
    }
}