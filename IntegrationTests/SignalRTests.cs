using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gate.Adapters.AspNet.IntegrationTests.Internal;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Xunit;

namespace Gate.Adapters.AspNet.IntegrationTests {
    public class SignalRTests : IDisposable {
        private readonly HubConnection _connection;
        private readonly IHubProxy _hub;
        
        public SignalRTests() {
            _connection = new HubConnection(TestHost.Url.ToString());
            _hub = _connection.CreateHubProxy("TestHub");
            _connection.Start().Wait();
        }

        [Fact]
        public async Task String_Roundtrip() {
            var result = await _hub.Invoke<string>("Roundtrip", "ABCDEF");
            Assert.Equal("ABCDEF", result);
        }

        public void Dispose() {
            this._connection.Stop();
        }
    }
}
