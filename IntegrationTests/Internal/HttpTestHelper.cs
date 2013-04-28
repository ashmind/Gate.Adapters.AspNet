using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Gate.Adapters.AspNetMvc.IntegrationTests.Internal {
    public static class HttpTestHelper {
        public static string GetString(string url) {
            var request = WebRequest.CreateHttp(TestHost.Url + url);
            request.Method = "GET";

            HttpWebResponse response;
            try {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex) {
                if (ex.Status != WebExceptionStatus.ProtocolError)
                    throw;

                response = (HttpWebResponse)ex.Response;
                var message = string.Format("Request failed: ({0}) {1}{2}{3}.",
                                            ((int)response.StatusCode),
                                            response.StatusDescription,
                                            Environment.NewLine,
                                            ReadResponseText(response));
                throw new Exception(message);
            }

            return ReadResponseText(response);
        }

        private static string ReadResponseText(HttpWebResponse response) {
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }
}
