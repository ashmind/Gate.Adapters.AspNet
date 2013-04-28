using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Gate.Adapters.AspNetMvc.IntegrationTests.WebSite.Controllers
{
    public class TestController : Controller
    {
        public ActionResult ContentFromQueryString([QueryString] string content)
        {
            return this.Content(content, "text/plain");
        }
    }
}
