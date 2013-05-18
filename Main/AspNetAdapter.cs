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
        private readonly CrossAppDomainDataConverter _converter;

        public AspNetAdapter(Func<IDictionary<string, object>, Task> next, AspNetAdapterArguments arguments) {
            _next = next;
            _remote = this.CreateAspNetRemote(Argument.NotNull("arguments", arguments));
            _converter = new CrossAppDomainDataConverter();
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
            Task.Factory.StartNew(() => _remote.ProcessRequest(_converter.CreateRequestData(environment)))
                        .ContinueWith(t => {
                            if (t.Exception != null) {
                                parent.SetException(t.Exception);
                                return;
                            }

                            this.ProcessResponse(environment, t.Result, parent);
                        });

            return parent.Task;
        }

        private void ProcessResponse(IDictionary<string, object> environment, CrossAppDomainResponseData responseData, TaskCompletionSource<object> parent) {
            if (responseData.StatusCode == 404) {
                _next(environment).ContinueWith(t => {
                    if (t.Exception != null) {
                        parent.SetException(t.Exception);
                        return;
                    }

                    parent.SetResult(null);
                });
                return;
            }

            try {
                _converter.UpdateWithResponseData(environment, responseData);
            }
            catch (Exception ex) {
                parent.SetException(ex);
                return;
            }

            parent.SetResult(null);
        }
    }
}
