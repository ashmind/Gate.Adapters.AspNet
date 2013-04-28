using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Hosting;
using Gate.Adapters.AspNet.Integration;

namespace Gate.Adapters.AspNet {
    public class AspNetAdapter {
        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly AspNetRemote _remote;

        public AspNetAdapter(Func<IDictionary<string, object>, Task> next, string appPhysicalPath) {
            this._next = next;
            this._remote = this.CreateAspNetRemote(appPhysicalPath);
        }

        private AspNetRemote CreateAspNetRemote(string appPhysicalPath) {
            // pretty terrible, any better ideas?
            var assembly = Assembly.GetExecutingAssembly();
            File.Copy(assembly.Location, Path.Combine(appPhysicalPath, "bin", Path.GetFileName(assembly.Location)), true);
            // might be useful to copy dependencies as well

            return (AspNetRemote)ApplicationHost.CreateApplicationHost(typeof(AspNetRemote), "/", appPhysicalPath);
        }

        public Task Invoke(IDictionary<string, object> environment) {
            var parent = new TaskCompletionSource<object>();
            Task.Factory.StartNew(() => this._remote.ProcessRequest(this.CreateRequestData(environment)))
                        .ContinueWith(t => {
                            if (t.Exception != null) {
                                parent.SetException(t.Exception);
                                return;
                            }

                            this.ProcessResponse(environment, t.Result, parent);
                        });

            return parent.Task;
        }

        private CrossDomainRequestData CreateRequestData(IDictionary<string, object> environment) {
            var request = new Request(environment);
            var rawUrl = request.Path + "?" + request.QueryString;

            return new CrossDomainRequestData(rawUrl,
                                              request.Path,
                                              request.QueryString,
                                              request.Method,
                                              request.Version);
        }

        private void ProcessResponse(IDictionary<string, object> environment, CrossDomainResponseData responseData, TaskCompletionSource<object> parent) {
            if (responseData.StatusCode == 404) {
                this._next(environment).ContinueWith(t => {
                    if (t.Exception != null) {
                        parent.SetException(t.Exception);
                        return;
                    }

                    parent.SetResult(null);
                });
            }

            try {
                this.UpdateResponseData(responseData, environment);
            }
            catch (Exception ex) {
                parent.SetException(ex);
                return;
            }

            parent.SetResult(null);
        }

        private void UpdateResponseData(CrossDomainResponseData responseData, IDictionary<string, object> environment) {
            var response = new Response(environment);
            response.StatusCode = responseData.StatusCode;
            response.ReasonPhrase = responseData.StatusDescription;

            foreach (var pair in responseData.Headers) {
                response.Headers.Add(pair.Key, new[] { pair.Value });
            }

            foreach (var data in responseData.MemoryData) {
                response.Write(data.Item1, 0, data.Item2);
            }
        }
    }
}
