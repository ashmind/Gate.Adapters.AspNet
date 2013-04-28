using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class RazorViewWithoutBuildManager : RazorView {
        public RazorViewWithoutBuildManager(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions) 
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions)
        {
        }

        public RazorViewWithoutBuildManager(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator) 
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance) {
            throw new NotImplementedException();
        }
    }
}
