using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace Gate.Adapters.AspNet.TestWebSite.Hubs {
    public class TestHub : Hub {
        public string Roundtrip(string value) {
            return value;
        }
    }
}