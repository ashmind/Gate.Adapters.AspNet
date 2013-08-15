using System;
using System.Collections.Generic;
using System.IO;
using Owin;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public class TestHostStartup {
        public void Configuration(IAppBuilder app) {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //CodeBase references where the assembly originally was stored. Location pointed to somewhere Temp
            var webSitePath = Path.Combine(Path.GetDirectoryName(assembly.CodeBase), @"..\..\..\TestWebSite");
            webSitePath = Path.GetFullPath(new Uri(webSitePath).LocalPath);

            var testApplicationData = new Dictionary<string, object> {
                { "Test.Data", TestHost.TestApplicationDataValue },
                { "Test.Data.Provider", new TestStringProvider(TestHost.TestApplicationDataProviderValue) }
            };
            app.Use(typeof(AspNetAdapter), new AspNetAdapterArguments(webSitePath, testApplicationData));
        }
    }
}