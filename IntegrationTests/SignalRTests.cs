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
        public async Task Roundtrip() {
            var result = await _hub.Invoke<string>("Roundtrip", "ABCDEF");
            Assert.Equal("ABCDEF", result);
        }

        [Fact]
        public async Task Roundtrip_MultipleTimes() {
            await _hub.Invoke<string>("Roundtrip", "First");
            await _hub.Invoke<string>("Roundtrip", "Second");

            var result = await _hub.Invoke<string>("Roundtrip", "Third");
            Assert.Equal("Third", result);
        }

        [Fact]
        public async Task Event() {
            var eventResult = new TaskCompletionSource<string>();
            
            _hub.On("Event", s => eventResult.SetResult(s));
            await _hub.Invoke<string>("RaiseEvent", "ABCDEF");

            Assert.Equal("ABCDEF", await eventResult.Task);
        }

        public void Dispose() {
            this._connection.Stop();
        }
    }
}
