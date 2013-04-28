using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpContext : HttpContextBase {
        private readonly GateHttpRequest _request;
        private readonly GateHttpResponse _response;

        public GateHttpContext(GateHttpRequest request, GateHttpResponse response) {
            _request = request;
            _response = response;
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
