using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossAppDomainRequestData {
        public CrossAppDomainRequestData(string httpVersion,
                                         string httpVerbName,
                                         CrossAppDomainAddressAndPort local,
                                         CrossAppDomainAddressAndPort remote,
                                         string rawUrl,
                                         string uriPath,
                                         string queryString,
                                         IDictionary<string, string> headers,
                                         byte[] body) 
        {
            HttpVersion = httpVersion;
            HttpVerbName = httpVerbName;
            Local = Argument.NotNull("local", local);
            Remote = Argument.NotNull("remote", remote);
            RawUrl = rawUrl;
            UriPath = uriPath;
            QueryString = queryString;
            Headers = new ReadOnlyDictionary<string, string>(Argument.NotNull("headers", headers));
            Body = body;
        }
        public string RawUrl { get; private set; }
        public string UriPath { get; private set; }
        public string QueryString { get; private set; }
        public string HttpVerbName { get; private set; }
        public CrossAppDomainAddressAndPort Local { get; private set; }
        public CrossAppDomainAddressAndPort Remote { get; private set; }
        public string HttpVersion { get; private set; }
        public IReadOnlyDictionary<string, string> Headers { get; private set; }
        public byte[] Body { get; private set; }
    }
}
