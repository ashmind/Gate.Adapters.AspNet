using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public static class HttpTestHelper {
        private static readonly HttpClient Client = new HttpClient { BaseAddress = TestHost.Url };

        public static string GetString(string url) {
            return GetString(Client.GetAsync(url));
        }

        public static string PostAndGetString(string url, HttpContent content) {
            return GetString(Client.PostAsync(url, content));
        }

        private static string GetString(Task<HttpResponseMessage> request) {
            var response = request.Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode) {
                var message = string.Format("Request failed: ({0}) {1}{2}{3}.",
                                            ((int)response.StatusCode),
                                            response.ReasonPhrase,
                                            Environment.NewLine,
                                            content);
                throw new Exception(message);
            }

            return content;
        }
    }
}
