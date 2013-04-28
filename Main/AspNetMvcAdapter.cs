using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Gate.Adapters.AspNetMvc.Integration;

namespace Gate.Adapters.AspNetMvc
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class AspNetMvcAdapter
    {
        private readonly AppFunc _next;
        private readonly string _appPhysicalPath;

        public AspNetMvcAdapter(AppFunc next, string appPhysicalPath, HttpApplication application) {
            Argument.NotNull("application", application);
            //this.host = (AspNetMvcHost)ApplicationHost.CreateApplicationHost(typeof(AspNetMvcHost), "", appPhysicalPath);

            OverrideMvcServicesBeforeStart(appPhysicalPath);
            RunApplicationStart(application);
            OverrideMvcServicesAfterStart();

            _next = next;
        }

        private void OverrideMvcServicesBeforeStart(string appPhysicalPath) {
            BundleTable.MapPathMethod = path => Regex.Replace(path, "^~", appPhysicalPath);
        }

        private void RunApplicationStart(HttpApplication application) {
            var start = application.GetType().GetMethod("Application_Start", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (start == null) // ???
                return;

            start.Invoke(application, null);
        }

        private void OverrideMvcServicesAfterStart() {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            if (controllerFactory is DefaultControllerFactory)
                ControllerBuilder.Current.SetControllerFactory(new ControllerFactoryWithoutBuildManager());

            var factories = ValueProviderFactories.Factories;
            for (var i = 0; i < factories.Count; i++) {
                if (factories[i] is FormValueProviderFactory)
                    factories[i] = new ValueProviderFactoryWithoutSupportForUnvalidated(r => r.Form);

                if (factories[i] is QueryStringValueProviderFactory)
                    factories[i] = new ValueProviderFactoryWithoutSupportForUnvalidated(r => r.QueryString);
            }
        }

        public Task Invoke(IDictionary<string, object> environment) {
            var httpContext = new GateHttpContext(new GateHttpRequest(new Request(environment)),
                                                  new GateHttpResponse(new Response(environment)));

            var routeData = RouteTable.Routes.GetRouteData(httpContext);
            if (routeData == null) {
                httpContext.Response.StatusCode = 404;
                return _next(environment);
            }

            var requestContext = new RequestContext(httpContext, routeData);
            return new MvcHandlerAccessor(requestContext).ProcessRequestAsync(httpContext);
        }
    }
}
