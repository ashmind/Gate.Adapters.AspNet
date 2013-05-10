using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Gate.Adapters.AspNet.Integration;

namespace Gate.Adapters.AspNet {
    public class AspNetRemote : MarshalByRefObject {
        public CrossDomainResponseData ProcessRequest(CrossDomainRequestData requestData) {
            var responseData = new CrossDomainResponseData();
            HttpRuntime.ProcessRequest(new GateWorkerRequest(requestData, responseData));
            return responseData;
        }

        public void SetApplicationData(IReadOnlyDictionary<string, object> applicationData) {
            // creating fake context is the only way to access HttpApplicationFactory.ApplicationState
            var context = new HttpContext(new SimpleWorkerRequest("", "", new StringWriter()));
            foreach (var pair in applicationData) {
                context.Application[pair.Key] = pair.Value;
            }
        }

        public override object InitializeLifetimeService() {
            // live forever!
            return null;
        }
    }
}