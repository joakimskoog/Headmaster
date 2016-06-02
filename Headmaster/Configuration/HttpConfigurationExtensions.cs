using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Headmaster.Configuration
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration EnableHeaderVersioning(this HttpConfiguration configuration, HeaderVersioningOptions options)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var cache = new HttpControllerDescriptorCache(configuration, options);
            var controllerSelector = new AcceptHeaderControllerSelector(cache);
            configuration.Services.Replace(typeof(IHttpControllerSelector), controllerSelector);

            return configuration;
        }
    }
}