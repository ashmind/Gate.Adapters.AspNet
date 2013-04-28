using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gate.Adapters.AspNetMvc.Integration {
    // DefaultControllerFactory uses BuildManager which is currently not supported
    public class ControllerFactoryWithoutBuildManager : DefaultControllerFactory {
        private readonly Lazy<IDictionary<string, Type[]>> _typeCache = new Lazy<IDictionary<string, Type[]>>(BuildTypeCache, LazyThreadSafetyMode.ExecutionAndPublication);

        protected override Type GetControllerType(RequestContext requestContext, string controllerName) {
            Argument.NotNullOrEmpty("controllerName", controllerName);
            Type[] types;
            _typeCache.Value.TryGetValue(controllerName, out types);

            if (types == null || types.Length == 0)
                return null;

            if (types.Length > 1) {
                var contollerList = string.Join(Environment.NewLine, types.AsEnumerable());
                var message = string.Format("Multiple types were found that match the controller named '{0}'.{1}The request has found the following matching controllers:{1}{2}",
                                            controllerName, Environment.NewLine, contollerList);
                throw new InvalidOperationException(message);
            }
            
            return types[0];
        }

        private static IDictionary<string, Type[]> BuildTypeCache() {
            var typeGroups = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                             from type in GetExportedTypesIfPossible(assembly)
                             where typeof(IController).IsAssignableFrom(type)
                                && type.Name.EndsWith("Controller")
                                && !type.IsAbstract
                             let controllerName = Regex.Match(type.Name, "(.+)Controller$").Groups[1].Value
                             group type by controllerName;

            return typeGroups.ToDictionary(g => g.Key, g => g.ToArray(), StringComparer.InvariantCultureIgnoreCase);
        }

        private static Type[] GetExportedTypesIfPossible(Assembly assembly) {
            try {
                return assembly.GetExportedTypes();
            }
            catch (NotSupportedException) {
                return Type.EmptyTypes;
            }
        }
    }
}
