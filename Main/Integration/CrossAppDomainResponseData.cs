using System;
using System.Collections.Generic;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossAppDomainResponseData {
        public CrossAppDomainResponseData() {
            Body = new List<Tuple<byte[], int>>();
            Headers = new Dictionary<string, string>();
        }

        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public IDictionary<string, string> Headers { get; private set; }
        public IList<Tuple<byte[], int>> Body { get; private set; }
    }
}
