using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Routing;
using Gate.Adapters.AspNetMvc.Integration;

namespace Gate.Adapters.AspNetMvc
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class AspNetMvcAdapter
    {
        private readonly AppFunc _next;
        private readonly AspNetMvcInitializer _initializer;

        public AspNetMvcAdapter(AppFunc next, AspNetMvcInitializer initializer) {
            _next = next;
            _initializer = initializer;
            _initializer.Initialize();
        }

        public Task Invoke(IDictionary<string, object> environment) {
            var httpContext = new GateHttpContext(new GateHttpServerUtility(_initializer.AppPhysicalPath), 
                                                  new GateHttpRequest(new Request(environment)),
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
