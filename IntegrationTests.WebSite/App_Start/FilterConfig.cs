using System.Web.Mvc;

namespace Gate.Adapters.AspNetMvc.IntegrationTests.WebSite.App_Start {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}