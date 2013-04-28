using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class OwinEnvironment {
        private readonly IDictionary<string, object> untyped;

        public OwinEnvironment(IDictionary<string, object> untyped) {
            this.untyped = untyped;
        }

        public string RequestMethod {
            get { return this.GetValue<string>(); }
        }

        public string RequestScheme {
            get { return this.GetValue<string>(); }
        }

        public string RequestPathBase {
            get { return this.GetValue<string>(); }
        }

        public string RequestPath {
            get { return this.GetValue<string>(); }
        }

        public string RequestQueryString {
            get { return this.GetValue<string>(); }
        }

        public IDictionary<string, string[]> RequestHeaders {
            get { return this.GetValue<IDictionary<string, string[]>>(); }
        }

        public Stream RequestBody {
            get { return this.GetValue<Stream>(); }
        }

        public IDictionary<string, string[]> ResponseHeaders {
            get { return this.GetValue<IDictionary<string, string[]>>(); }
        }

        public Stream ResponseBody {
            get { return this.GetValue<Stream>(); }
        }
        
        private T GetValue<T>(string prefix = "owin", [CallerMemberName] string name = null) {
            object value;
            if (!this.untyped.TryGetValue(prefix + "." + name, out value))
                return default(T);

            return (T)value;
        }
    }
}
