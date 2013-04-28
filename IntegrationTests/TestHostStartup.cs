using System.IO;
using Gate.Adapters.AspNetMvc.IntegrationTests.WebSite;
using Owin;

namespace Gate.Adapters.AspNetMvc.IntegrationTests {
    public class TestHostStartup {
        public void Configuration(IAppBuilder app) {
            var webSitePath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), @"..\..\..\IntegrationTests.WebSite");
            app.Use(typeof(AspNetMvcAdapter), webSitePath, new MvcApplication());
        }
    }
}