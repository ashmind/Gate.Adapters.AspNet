using System.IO;
using Owin;

namespace Gate.Adapters.AspNet.TestConsoleHost {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            var webSitePath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), @"..\..\..\TestWebSite");
            webSitePath = Path.GetFullPath(webSitePath);
            app.Use(typeof(AspNetAdapter), new AspNetAdapterArguments(webSitePath));
        }
    }
}