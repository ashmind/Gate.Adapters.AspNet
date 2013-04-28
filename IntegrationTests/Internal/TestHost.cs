using System;
using Microsoft.Owin.Hosting;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public static class TestHost {
        private const string UrlInternal = "http://localhost:8087";

        // note: must not expose constant as we want the static constructor to run
        // when this is accessed
        public static string Url {
            get { return UrlInternal; }
        }

        static TestHost()  {
            var server = WebApplication.Start<TestHostStartup>(UrlInternal);
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => server.Dispose();
        }
    }
}