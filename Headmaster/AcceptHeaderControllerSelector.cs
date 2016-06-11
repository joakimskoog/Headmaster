using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using Headmaster.Configuration;
using Headmaster.Core;

namespace Headmaster
{
    public class AcceptHeaderControllerSelector : IHttpControllerSelector
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "actions";
        private const string SubRoutesKey = "MS_SubRoutes";

        private readonly HttpControllerDescriptorCache _controllerDescriptorCache;
        private readonly HeaderVersioningOptions _options;

        public AcceptHeaderControllerSelector(HttpControllerDescriptorCache controllerDescriptorCache, HeaderVersioningOptions options)
        {
            if (controllerDescriptorCache == null) throw new ArgumentNullException(nameof(controllerDescriptorCache));
            if (options == null) throw new ArgumentNullException(nameof(options));
            _controllerDescriptorCache = controllerDescriptorCache;
            _options = options;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor controllerDescriptor = null;

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var version = request.GetApiVersion(_options.MediaType, _options.MediaTypeParameter);
            if (string.IsNullOrEmpty(version) && _options.DefaultVersionResolving == DefaultVersionResolving.ThrowErrorIfEmpty)
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, "No API version was found"));
            }

            if (!string.IsNullOrEmpty(version))
            {
                version = _options.RequestVersionTransformer(version);
            }

            var subRoutes = routeData.GetSubRoutes();

            if (subRoutes == null)
            {
                string controllerName = GetRouteVariable<string>(routeData, ControllerKey);
                if (controllerName == null)
                {
                    throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, "Failed to find controller name"));
                }

                if (_controllerDescriptorCache.TryGet(controllerName, version, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
            }
            else
            {
                //We're using attribute routing
                var filteredSubRoutes = subRoutes.Where(routeAttributeData =>
                {
                    var currentDescriptor = GetControllerDescriptor(routeAttributeData);

                    if (string.IsNullOrEmpty(version))
                    {
                        string latestVersion = "";
                        if (_controllerDescriptorCache.TryGetLatestVersion(currentDescriptor.ControllerName, out latestVersion))
                        {
                            version = latestVersion;
                        }
                    }

                    bool hasSupportForVersion = currentDescriptor.HasSupportForVersion(version);
                    if (hasSupportForVersion && controllerDescriptor == null)
                    {
                        controllerDescriptor = currentDescriptor;
                    }

                    return hasSupportForVersion;
                });

                routeData.Values[SubRoutesKey] = filteredSubRoutes.ToArray();
                return controllerDescriptor;
            }

            throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, "Failed to find a controller that matches the request"));
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllerDescriptorCache.ControllerDescriptors;
        }

        private HttpControllerDescriptor GetControllerDescriptor(IHttpRouteData routeData)
        {
            return ((HttpActionDescriptor[])routeData.Route.DataTokens[ActionKey]).FirstOrDefault()?.ControllerDescriptor;
        }

        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }
    }
}