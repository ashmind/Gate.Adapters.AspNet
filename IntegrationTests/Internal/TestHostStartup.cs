using System.IO;
using Owin;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public class TestHostStartup {
        public void Configuration(IAppBuilder app) {
            var webSitePath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), @"..\..\..\IntegrationTests.WebSite");
            webSitePath = Path.GetFullPath(webSitePath);
            app.Use(typeof(AspNetAdapter), webSitePath);
        }
    }
}