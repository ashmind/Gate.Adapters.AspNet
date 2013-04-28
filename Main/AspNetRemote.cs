using System;
using System.Collections.Generic;
using System.Web;
using Gate.Adapters.AspNet.Integration;

namespace Gate.Adapters.AspNet {
    public class AspNetRemote : MarshalByRefObject {
        public CrossDomainResponseData ProcessRequest(CrossDomainRequestData requestData) {
            var responseData = new CrossDomainResponseData();
            HttpRuntime.ProcessRequest(new GateWorkerRequest(requestData, responseData));
            return responseData;
        }

        public override object InitializeLifetimeService() {
            // live forever!
            return null;
        }
    }
}