using System.Text.RegularExpressions;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpServerUtility : HttpServerUtilityBase {
        private readonly string _appPhysicalPath;

        public GateHttpServerUtility(string appPhysicalPath) {
            _appPhysicalPath = appPhysicalPath;
        }

        public override string MapPath(string path) {
            return Regex.Replace(path, "^~", _appPhysicalPath);
        }
    }
}