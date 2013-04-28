using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class MvcHandlerAccessor : MvcHandler {
        public MvcHandlerAccessor(RequestContext requestContext) : base(requestContext) {
        }

        public new void ProcessRequest(HttpContextBase httpContext) {
            base.ProcessRequest(httpContext);
        }

        public Task ProcessRequestAsync(HttpContextBase context) {
            return Task.Factory.FromAsync(this.BeginProcessRequest, this.EndProcessRequest, arg1: context, state: null);
        }
    }
}
