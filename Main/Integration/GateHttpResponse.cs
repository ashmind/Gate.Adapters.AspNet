using System;
using System.IO;
using System.Text;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpResponse : HttpResponseBase {
        private Response _response;

        public GateHttpResponse(Response response) {
            _response = response;
        }

        public override string ContentType {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public override Encoding ContentEncoding {
            get { return _response.Encoding; }
            set { _response.Encoding = value; }
        }

        public override void AppendHeader(string name, string value) {
            _response.Headers[name] = new[] { value };
        }

        public override void Write(object obj) {
            _response.Write((obj ?? "").ToString());
        }

        public override void Write(string s) {
            _response.Write(s);
        }

        public override void Write(char ch) {
            throw new NotSupportedException("That would be REALLY slow.");
        }

        public override void Write(char[] buffer, int index, int count) {
            _response.Write(new string(buffer, index, count));
        }

        public override void Flush() {
        }
    }
}