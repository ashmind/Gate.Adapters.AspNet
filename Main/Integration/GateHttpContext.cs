using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpContext : HttpContextBase {
        private readonly IDictionary _items;
        private readonly GateHttpServerUtility _server;
        private readonly GateHttpRequest _request;
        private readonly GateHttpResponse _response;

        public GateHttpContext(GateHttpServerUtility server, GateHttpRequest request, GateHttpResponse response) {
            // As far as I remember Items["X"] returns null, which is Hashtable semantics.
            // Dictionary<string, object> would throw instead.
            _items = new Hashtable();

            _server = Argument.NotNull("server", server);
            _request = Argument.NotNull("request", request);
            _response = Argument.NotNull("response", response);
        }

        public override IDictionary Items {
            get { return _items; }
        }

        public override HttpServerUtilityBase Server {
            get { return _server; }
        }

        public override HttpRequestBase Request {
            get { return _request; }
        }

        public override HttpResponseBase Response {
            get { return _response; }
        }

        public override HttpSessionStateBase Session {
            get { return null; }
        }
    }
}
