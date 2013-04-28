using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Gate.Adapters.AspNetMvc.Integration;

namespace Gate.Adapters.AspNetMvc {
    public class AspNetMvcInitializer {
        public string AppPhysicalPath { get; private set; }
        private readonly HttpApplication _application;

        public AspNetMvcInitializer(string appPhysicalPath, HttpApplication application) {
            AppPhysicalPath = Argument.NotNullOrEmpty("appPhysicalPath", appPhysicalPath);
            _application = Argument.NotNull("application", application); ;
        }

        public virtual void Initialize() {
            OverrideMvcServicesBeforeStart();
            RunApplicationStart();
            OverrideMvcServicesAfterStart();
        }

        protected virtual void OverrideMvcServicesBeforeStart() {
            BundleTable.MapPathMethod = path => Regex.Replace(path, "^~", AppPhysicalPath);
        }

        protected virtual void RunApplicationStart() {
            var start = _application.GetType().GetMethod("Application_Start", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (start == null) // ???
                return;

            start.Invoke(_application, null);
        }

        protected virtual void OverrideMvcServicesAfterStart() {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            if (controllerFactory is DefaultControllerFactory)
                ControllerBuilder.Current.SetControllerFactory(new ControllerFactoryWithoutBuildManager());

            this.OverrideValueProviderFactories();
            this.OverrideViewEngines();
        }

        // ReSharper disable OperatorIsCanBeUsed
        // Not using `is` as these classes might be unsealed later on.
        protected virtual void OverrideValueProviderFactories() {
            var factories = ValueProviderFactories.Factories;
            for (var i = 0; i < factories.Count; i++) {
                if (factories[i].GetType() == typeof(FormValueProviderFactory))
                    factories[i] = new ValueProviderFactoryWithoutSupportForUnvalidated(r => r.Form);

                if (factories[i].GetType() == typeof(QueryStringValueProviderFactory))
                    factories[i] = new ValueProviderFactoryWithoutSupportForUnvalidated(r => r.QueryString);
            }
        }
        // ReSharper restore OperatorIsCanBeUsed

        protected virtual void OverrideViewEngines() {
            var engines = ViewEngines.Engines;
            for (var i = 0; i < engines.Count; i++) {
                if (engines[i].GetType() == typeof(RazorViewEngine)) {
                    engines[i] = new RazorViewEngineWithoutBuildManager();
                }
                else if (engines[i] is BuildManagerViewEngine) {
                    throw new NotSupportedException("ViewEngine " + engines[i] + " uses BuildManager and is not supported by the current hosting process.");
                }
            }
        }

    }
}
