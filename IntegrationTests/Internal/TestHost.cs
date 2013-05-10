using System;
using Microsoft.Owin.Hosting;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public static class TestHost {
        public const string TestApplicationDataKey = "Test.Data";
        public const string TestApplicationDataValue = "Magic";

        private static readonly Uri _url = new Uri("http://localhost:8087");

        // causes static constructor to run
        public static Uri Url {
            get { return _url; }
        }

        static TestHost()  {
            var server = WebApplication.Start<TestHostStartup>(_url.ToString());
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => server.Dispose();
        }
    }
}