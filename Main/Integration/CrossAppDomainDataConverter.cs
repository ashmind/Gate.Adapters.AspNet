using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Owin.Types;

namespace Gate.Adapters.AspNet.Integration {
    public class CrossAppDomainDataConverter {
        public CrossAppDomainRequestData CreateRequestData(IDictionary<string, object> environment) {
            var request = new OwinRequest(environment);
            
            var rawUrl = request.Path + "?" + request.QueryString;
            var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
            var body = new MemoryStream();
            request.Body.CopyTo(body);

            return new CrossAppDomainRequestData(request.Protocol, //TODO: What kind of version is meant? = HTTP-Version. Guess at Protocol
                                                 request.Method,
                                                 new CrossAppDomainAddressAndPort(request.LocalIpAddress, Convert.ToInt32(request.LocalPort)),
                                                 GetRemoteAddressAndPort(request),
                                                 rawUrl,
                                                 request.Path,
                                                 request.QueryString,
                                                 headers,
                                                 body.ToArray());
        }

        private static CrossAppDomainAddressAndPort GetRemoteAddressAndPort(OwinRequest request) {
            var address = request.RemoteIpAddress ?? "0.0.0.0";
            var portString = request.RemotePort;
            var port = !string.IsNullOrEmpty(portString) ? int.Parse(portString) : 0;
            return new CrossAppDomainAddressAndPort(address, port);
        }

        public async Task UpdateWithResponseData(IDictionary<string, object> environment, CrossAppDomainResponseData responseData) {
            var response = new OwinResponse(environment);
            response.StatusCode = responseData.StatusCode;
            response.ReasonPhrase = responseData.StatusDescription;

            foreach (var pair in responseData.Headers) {
                response.Headers.Add(pair.Key, new[] { pair.Value });
            }

            foreach (var data in responseData.Body) {
                await response.WriteAsync(data);
            }

            foreach (var file in responseData.Files) {
                await SendFileToResponse(response, file);
            }
        }

        private static async Task SendFileToResponse(OwinResponse response, CrossAppDomainResponseFile file) {
            if(response.CanSendFile)
            {
                await response.SendFileAsync(file.Path, file.Length, file.Offset, CancellationToken.None);
                return;
            }

            var buffer = new byte[file.Length - file.Offset];
            using (var stream = File.OpenRead(file.Path)) {
                stream.Seek(file.Offset, SeekOrigin.Begin);
                await stream.ReadAsync(buffer, 0, (int)file.Length);
            }
            await response.WriteAsync(buffer);
        }
    }
}
