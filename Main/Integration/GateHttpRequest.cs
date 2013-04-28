using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpRequest : HttpRequestBase {
        private readonly Request _request;
        private readonly GateHttpBrowserCapabilities _browser;
        private readonly NameValueCollection _headers;
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _queryString;
        private readonly NameValueCollection _form;
        private readonly GateHttpFileCollection _files;

        public GateHttpRequest(Request request) {
            _request = request;
            _browser = new GateHttpBrowserCapabilities();
            _headers = new NameValueCollection();
            foreach (var pair in request.Headers) {
                _headers[pair.Key] = string.Join(",", pair.Value);
            }

            _cookies = new HttpCookieCollection();
            foreach (var pair in request.Cookies) {
                _cookies.Add(new HttpCookie(pair.Key, pair.Value));
            }

            _queryString = new NameValueCollection();
            foreach (var pair in request.Query) {
                _queryString.Add(pair.Key, pair.Value);
            }

            _form = new NameValueCollection();
            foreach (var pair in request.ReadForm()) {
                _form.Add(pair.Key, pair.Value);
            }

            _files = new GateHttpFileCollection();
        }

        public override string AppRelativeCurrentExecutionFilePath {          
            get { return "~" + _request.Path; }
        }

        public override string ContentType {
            get { return _request.ContentType ?? ""; }
            set { throw new NotSupportedException("Request cannot be changed."); }
        }

        public override NameValueCollection Headers {
            get { return _headers; }
        }

        public override HttpBrowserCapabilitiesBase Browser {
            get { return _browser; }
        }

        public override string UserAgent {
            get { return this.Headers["User-Agent"]; }
        }

        public override HttpCookieCollection Cookies {
            get { return _cookies; }
        }

        public override string PathInfo {
            get { return ""; }
        }

        public override NameValueCollection QueryString {
            get { return _queryString; }
        }

        public override NameValueCollection Form {
            get { return _form; }
        }

        public override HttpFileCollectionBase Files {
            get { return _files; }
        }

        public override void ValidateInput() {
            //  do nothing for now
        }
    }
}
