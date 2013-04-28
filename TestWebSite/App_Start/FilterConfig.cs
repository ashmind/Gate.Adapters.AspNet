using System.Web.Mvc;

namespace Gate.Adapters.AspNet.TestWebSite.App_Start {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}