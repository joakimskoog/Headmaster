using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Headmaster
{
    public class AcceptHeaderControllerSelector : IHttpControllerSelector
    {
        private readonly HttpControllerDescriptorCache _controllerDescriptorCache;

        public AcceptHeaderControllerSelector(HttpControllerDescriptorCache controllerDescriptorCache)
        {
            if (controllerDescriptorCache == null) throw new ArgumentNullException(nameof(controllerDescriptorCache));
            _controllerDescriptorCache = controllerDescriptorCache;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            throw new System.NotImplementedException();
        }
    }
}