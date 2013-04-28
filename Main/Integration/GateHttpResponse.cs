using System;
using System.IO;
using System.Text;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpResponse : HttpResponseBase {
        private Response _response;
        // not sent currently
        private readonly HttpCookieCollection _cookies;
        private TextWriter _output;

        public GateHttpResponse(Response response) {
            _response = response;
            _cookies = new HttpCookieCollection();
            _output = new StreamWriter(_response.OutputStream, _response.Encoding) { AutoFlush = true };
        }

        public override string ContentType {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public override Encoding ContentEncoding {
            get { return _response.Encoding; }
            set {
                if (Equals(value, _response.Encoding))
                    return;

                _output.Flush();
                _response.Encoding = value;
                _output = new StreamWriter(_response.OutputStream, _response.Encoding) { AutoFlush = true };
            }
        }

        public override HttpCookieCollection Cookies {
            get { return _cookies; }
        }

        public override TextWriter Output {
            get { return _output; }
            set { throw new NotSupportedException("This is not currently supported."); }
        }

        public override void AppendHeader(string name, string value) {
            _response.Headers[name] = new[] { value };
        }

        public override void Write(object obj) {
            _output.Write(obj);
        }

        public override void Write(string s) {
            _output.Write(s);
        }

        public override void Write(char ch) {
            _output.Write(ch);
        }

        public override void Write(char[] buffer, int index, int count) {
            _output.Write(buffer, index, count);
        }

        public override void Flush() {
        }

        public override void End() {
        }
    }
}