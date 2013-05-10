using System;
using System.Collections.Generic;
using System.Linq;
using TestWebSite.Dependencies;

namespace Gate.Adapters.AspNet.IntegrationTests.Internal {
    public class TestStringProvider : MarshalByRefObject, IStringProvider {
        private readonly string _string;

        public TestStringProvider(string @string) {
            _string = @string;
        }

        public string GetString() {
            return _string;
        }
    }
}
