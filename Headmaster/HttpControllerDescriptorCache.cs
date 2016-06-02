using System;
using System.Web.Http;
using Headmaster.Configuration;

namespace Headmaster
{
    public sealed class HttpControllerDescriptorCache
    {
        private readonly HttpConfiguration _configuration;
        private readonly HeaderVersioningOptions _options;

        public HttpControllerDescriptorCache(HttpConfiguration configuration, HeaderVersioningOptions options)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (options == null) throw new ArgumentNullException(nameof(options));
            _configuration = configuration;
            _options = options;
        }
    }
}