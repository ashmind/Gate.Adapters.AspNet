using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gate.Adapters.AspNet.Integration {
    public class CrossAppDomainDataConverter {
        public CrossAppDomainRequestData CreateRequestData(IDictionary<string, object> environment) {
            var request = new Request(environment);
            
            var rawUrl = request.Path + "?" + request.QueryString;
            var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
            var body = new MemoryStream();
            request.Body.CopyTo(body);

            return new CrossAppDomainRequestData(request.Version,
                                                 request.Method,
                                                 new CrossAppDomainAddressAndPort(request.Host, request.Port),
                                                 GetRemoteAddressAndPort(request),
                                                 rawUrl,
                                                 request.Path,
                                                 request.QueryString,
                                                 headers,
                                                 body.ToArray());
        }

        private static CrossAppDomainAddressAndPort GetRemoteAddressAndPort(Request request) {
            var address = request.Get<string>("server.RemoteIpAddress") ?? "0.0.0.0";
            var portString = request.Get<string>("server.RemotePort");
            var port = !string.IsNullOrEmpty(portString) ? int.Parse(portString) : 0;
            return new CrossAppDomainAddressAndPort(address, port);
        }

        public void UpdateWithResponseData(IDictionary<string, object> environment, CrossAppDomainResponseData responseData) {
            var response = new Response(environment);
            response.StatusCode = responseData.StatusCode;
            response.ReasonPhrase = responseData.StatusDescription;

            foreach (var pair in responseData.Headers) {
                response.Headers.Add(pair.Key, new[] { pair.Value });
            }

            foreach (var data in responseData.Body) {
                response.Write(data.Item1, 0, data.Item2);
            }
        }
    }
}
