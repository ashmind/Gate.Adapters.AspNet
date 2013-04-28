using System;
using System.Collections.Generic;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossDomainRequestData {
        public CrossDomainRequestData(string rawUrl, string uriPath, string queryString, string httpVerbName, string httpVersion) {
            this.RawUrl = rawUrl;
            this.UriPath = uriPath;
            this.QueryString = queryString;
            this.HttpVerbName = httpVerbName;
            this.HttpVersion = httpVersion;
        }

        public string RawUrl { get; private set; }
        public string UriPath { get; private set; }
        public string QueryString { get; private set; }
        public string HttpVerbName { get; private set; }
        public string HttpVersion { get; private set; }
    }
}
