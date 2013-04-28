using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Gate.Adapters.AspNet.TestWebSite.Controllers {
    public class TestController : Controller {
        public ActionResult Index() {
            var actions = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return View(actions);
        }

        public ActionResult ContentFromQueryString([QueryString] string content) {
            return this.Content(content, "text/plain");
        }

        [HttpPost]
        public ActionResult ContentFromForm([Form] string content) {
            return this.Content(content, "text/plain");
        }

        public ActionResult ViewFromQueryString([QueryString] string content) {
            return this.View("String", (object)content);
        }
    }
}
