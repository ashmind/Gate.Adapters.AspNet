﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;
using System.Web.Mvc;
using TestWebSite.Dependencies;

namespace Gate.Adapters.AspNet.TestWebSite.Controllers {
    public class TestController : Controller {
        public ActionResult Index() {
            var actions = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return View(actions);
        }
        
        public ActionResult ContentFromQueryString([QueryString] string content) {
            return Content(content, "text/plain");
        }

        [HttpPost]
        public ActionResult ContentFromForm([Form] string content) {
            return Content(content, "text/plain");
        }

        public ActionResult ContentFromApplicationData() {
            var content = HttpContext.Application["Test.Data"] as string;
            return Content(content, "text/plain");
        }

        public ActionResult ContentFromApplicationDataRemotingObject() {
            var provider = (IStringProvider)HttpContext.Application["Test.Data.Provider"];
            return Content(provider.GetString(), "text/plain");
        }

        public ActionResult FileWithTextContent() {
            return File(Server.MapPath("~/Content/Test.txt"), "text/plain");
        }

        public ActionResult ViewFromQueryString([QueryString] string content) {
            return View("String", (object)content);
        }
    }
}
