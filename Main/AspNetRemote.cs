using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Gate.Adapters.AspNet.Integration;

namespace Gate.Adapters.AspNet {
    public class AspNetRemote : MarshalByRefObject {
        public CrossAppDomainResponseData ProcessRequest(CrossAppDomainRequestData requestData) {
            var responseData = new CrossAppDomainResponseData();
            // can be rewritten to support true async later on (beginrequest/endrequest)
            using (var request = new GateWorkerRequest(requestData, responseData)) {
                HttpRuntime.ProcessRequest(request);
                request.WaitForEnd();
            }
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