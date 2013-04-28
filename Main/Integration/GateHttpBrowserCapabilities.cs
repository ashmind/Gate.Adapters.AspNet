using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    public class GateHttpBrowserCapabilities : HttpBrowserCapabilitiesBase {
        public override bool IsMobileDevice {
            get { return false; } // not implemented
        }
    }
}