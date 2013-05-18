using System;
using System.Collections.Generic;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossAppDomainAddressAndPort {
        public CrossAppDomainAddressAndPort(string address, int port) {
            Address = address;
            Port = port;
        }

        public string Address { get; private set; }
        public int Port { get; private set; }
    }
}
