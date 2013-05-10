using System;
using System.Collections.Generic;
using System.Linq;

namespace Gate.Adapters.AspNet {
    [Serializable]
    public class AspNetAdapterArguments {
        public string ApplicationPhysicalPath { get; private set; }
        public IReadOnlyDictionary<string, object> ApplicationData { get; private set; }

        public AspNetAdapterArguments(string applicationPhysicalPath, IReadOnlyDictionary<string, object> applicationData = null) {
            ApplicationPhysicalPath = applicationPhysicalPath;
            ApplicationData = applicationData ?? new Dictionary<string, object>();
        }
    }
}
