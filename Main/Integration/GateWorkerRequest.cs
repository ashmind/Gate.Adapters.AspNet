using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gate.Adapters.AspNet.Integration {
    public class GateWorkerRequest : HttpWorkerRequest {
        private readonly CrossDomainRequestData _requestData;
        private readonly CrossDomainResponseData _responseData;

        public GateWorkerRequest(CrossDomainRequestData requestData, CrossDomainResponseData responseData) {
            this._requestData = requestData;
            this._responseData = responseData;
        }

        public override string GetUriPath() {
            return this._requestData.UriPath;
        }

        public override string GetQueryString() {
            return this._requestData.QueryString;
        }

        public override string GetRawUrl() {
            return this._requestData.RawUrl;
        }

        public override string GetHttpVerbName() {
            return this._requestData.HttpVerbName;
        }

        public override string GetHttpVersion() {
            return this._requestData.HttpVersion;
        }

        public override string GetRemoteAddress() {
            return "";
        }

        public override int GetRemotePort() {
            return 0;
        }

        public override string GetLocalAddress() {
            return "";
        }

        public override int GetLocalPort() {
            return 0;
        }

        public override void SendStatus(int statusCode, string statusDescription) {
            this._responseData.StatusCode = statusCode;
            this._responseData.StatusDescription = statusDescription;
        }

        public override void SendKnownResponseHeader(int index, string value) {
            var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);
            this._responseData.Headers.Add(name, value);
        }

        public override void SendUnknownResponseHeader(string name, string value) {
            this._responseData.Headers.Add(name, value);
        }

        public override void SendResponseFromMemory(byte[] data, int length) {
            this._responseData.MemoryData.Add(Tuple.Create(data, length));
        }

        public override void SendResponseFromFile(string filename, long offset, long length) {
            throw new NotImplementedException();
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length) {
            throw new NotImplementedException();
        }

        public override void FlushResponse(bool finalFlush) {
        }

        public override void EndOfRequest() {
            // ???
        }
    }
}
