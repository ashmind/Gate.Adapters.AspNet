using System;
using Microsoft.Owin.Hosting;

namespace Gate.Adapters.AspNetMvc.IntegrationTests {
    public static class TestHost {
        public const string Url = "http://localhost/8087";

        private static IDisposable server;

        public static void EnsureStarted() {
            server = WebApplication.Start<TestHostStartup>(Url);
        }
    }
}