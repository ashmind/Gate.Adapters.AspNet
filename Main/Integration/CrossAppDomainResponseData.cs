using System;
using System.Collections.Generic;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossAppDomainResponseData {
        public CrossAppDomainResponseData() {
            Headers = new Dictionary<string, string>();
            Body = new List<byte[]>();
            Files = new List<CrossAppDomainResponseFile>();
        }

        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public IDictionary<string, string> Headers { get; private set; }
        public IList<byte[]> Body { get; private set; }
        public IList<CrossAppDomainResponseFile> Files { get; private set; }
    }
}
