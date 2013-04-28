using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpRequest : HttpRequestBase {
        private readonly Request _request;
        private readonly NameValueCollection _queryString;
        private readonly NameValueCollection _form;
        private readonly GateHttpFileCollection _files;

        public GateHttpRequest(Request request) {
            _request = request;
            _form = new NameValueCollection();
            _queryString = new NameValueCollection();

            foreach (var pair in request.Query) {
                _queryString.Add(pair.Key, pair.Value);
            }

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
