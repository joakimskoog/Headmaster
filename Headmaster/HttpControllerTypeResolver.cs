using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;

namespace Headmaster
{
    public class HttpControllerTypeResolver : IControllerTypesResolver
    {
        private readonly HttpConfiguration _configuration;

        public HttpControllerTypeResolver(HttpConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _configuration = configuration;
        }

        public IReadOnlyCollection<Type> GetControllerTypes()
        {
            var assembliesResolver = _configuration.Services.GetAssembliesResolver();
            var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            return new ReadOnlyCollection<Type>(controllersResolver.GetControllerTypes(assembliesResolver).ToList());
        }
    }
}