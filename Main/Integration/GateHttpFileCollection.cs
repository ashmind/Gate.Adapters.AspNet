using System.Web;

namespace Gate.Adapters.AspNetMvc.Integration {
    // not supported yet
    public class GateHttpFileCollection : HttpFileCollectionBase {
        public override int Count {
            get { return 0; }
        }
    }
}