using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class RazorViewEngineWithoutBuildManager : RazorViewEngine {
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath) {
            var path = controllerContext.HttpContext.Server.MapPath(virtualPath);
            return File.Exists(path);
        }
    }
}
