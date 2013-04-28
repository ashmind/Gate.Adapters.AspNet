using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Gate.Adapters.AspNetMvc.Integration {
    // Default Form/QueryString factories use Validation.Unvalidated, see https://twitter.com/ashmind/status/328345408961134594
    internal class ValueProviderFactoryWithoutSupportForUnvalidated : ValueProviderFactory {
        private readonly Func<HttpRequestBase, NameValueCollection> _getCollection;

        public ValueProviderFactoryWithoutSupportForUnvalidated(Func<HttpRequestBase, NameValueCollection> getCollection) {
            _getCollection = Argument.NotNull("getCollection", getCollection);
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext) {
            return new NameValueCollectionValueProvider(_getCollection(controllerContext.HttpContext.Request), CultureInfo.CurrentCulture);
        }
    }
}