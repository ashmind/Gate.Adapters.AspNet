using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Gate.Adapters.AspNet.Integration {
    public class GateWorkerRequest : HttpWorkerRequest, IDisposable {
        private readonly CrossAppDomainRequestData _requestData;
        private readonly CrossAppDomainResponseData _responseData;
        private readonly ManualResetEventSlim  _ended;

        public GateWorkerRequest(CrossAppDomainRequestData requestData, CrossAppDomainResponseData responseData) {
            _requestData = requestData;
            _responseData = responseData;
            _ended = new ManualResetEventSlim();
        }

        public override string GetHttpVerbName() {
            return _requestData.HttpVerbName;
        }

        public override string GetHttpVersion() {
            return _requestData.HttpVersion;
        }

        public override string GetRawUrl() {
            return _requestData.RawUrl;
        }

        public override string GetUriPath() {
            return _requestData.UriPath;
        }

        public override string GetQueryString() {
            return _requestData.QueryString;
        }

        public override string GetKnownRequestHeader(int index) {
            var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);
            string value;
            if (!_requestData.Headers.TryGetValue(name, out value))
                return null;

            return value;
        }

        public override byte[] GetPreloadedEntityBody() {
            return _requestData.Body;
        }

        public override string GetRemoteAddress() {
            return _requestData.Remote.Address;
        }

        public override int GetRemotePort() {
            return _requestData.Remote.Port;
        }

        public override string GetLocalAddress() {
            return _requestData.Local.Address;
        }

        public override int GetLocalPort() {
            return _requestData.Local.Port;
        }

        public override void SendStatus(int statusCode, string statusDescription) {
            _responseData.StatusCode = statusCode;
            _responseData.StatusDescription = statusDescription;
        }

        public override void SendKnownResponseHeader(int index, string value) {
            var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);
            _responseData.Headers.Add(name, value);
        }

        public override void SendUnknownResponseHeader(string name, string value) {
            _responseData.Headers.Add(name, value);
        }

        public override void SendResponseFromMemory(byte[] data, int length) {
            _responseData.Body.Add(Tuple.Create(data, length));
        }

        public override void SendResponseFromFile(string filename, long offset, long length) {
            throw new NotImplementedException("SendResponseFromFile is not implemented.");
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length) {
            throw new NotImplementedException("SendResponseFromFile is not implemented.");
        }

        public override void FlushResponse(bool finalFlush) {
        }

        public void WaitForEnd() {
            _ended.Wait();
        }

        public override void EndOfRequest() {
            _ended.Set();
        }

        public void Dispose() {
            _ended.Dispose();
        }
    }
}
