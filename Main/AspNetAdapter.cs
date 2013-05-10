﻿using System;
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

        public AspNetAdapter(Func<IDictionary<string, object>, Task> next, AspNetAdapterArguments arguments) {
            this._next = next;
            this._remote = this.CreateAspNetRemote(Argument.NotNull("arguments", arguments));
        }

        private AspNetRemote CreateAspNetRemote(AspNetAdapterArguments arguments) {
            // pretty terrible, any better ideas?
            var assembly = Assembly.GetExecutingAssembly();
            File.Copy(assembly.Location, Path.Combine(arguments.ApplicationPhysicalPath, "bin", Path.GetFileName(assembly.Location)), true);
            // might be useful to copy dependencies as well

            var remote = (AspNetRemote)ApplicationHost.CreateApplicationHost(typeof(AspNetRemote), "/", arguments.ApplicationPhysicalPath);
            remote.SetApplicationData(arguments.ApplicationData);
            return remote;
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
            var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
            var body = new MemoryStream();
            request.Body.CopyTo(body);

            return new CrossDomainRequestData(request.Version,
                                              request.Method,
                                              rawUrl,
                                              request.Path,
                                              request.QueryString,
                                              headers,
                                              body.ToArray());
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
                return;
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

            foreach (var data in responseData.Body) {
                response.Write(data.Item1, 0, data.Item2);
            }
        }
    }
}
