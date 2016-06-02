using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Headmaster.Configuration;
using Headmaster.Core;

namespace Headmaster
{
    public sealed class HttpControllerDescriptorCache
    {
        private readonly HttpConfiguration _configuration;
        private readonly HeaderVersioningOptions _options;
        private readonly IControllerTypesResolver _controllerTypesResolver;

        private readonly Lazy<IDictionary<string, HttpControllerDescriptor>> _controllers;

        public IDictionary<string, HttpControllerDescriptor> ControllerDescriptors => _controllers.Value;

        public HttpControllerDescriptorCache(HttpConfiguration configuration, HeaderVersioningOptions options, IControllerTypesResolver controllerTypesResolver)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (controllerTypesResolver == null) throw new ArgumentNullException(nameof(controllerTypesResolver));
            _configuration = configuration;
            _options = options;
            _controllerTypesResolver = controllerTypesResolver;
            _controllers = new Lazy<IDictionary<string, HttpControllerDescriptor>>(InitialiseControllerDictionary);
        }

        private IDictionary<string, HttpControllerDescriptor> InitialiseControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var controllerType in _controllerTypesResolver.GetControllerTypes())
            {
                //Strip "Controller" from the end of the type name.
                //This matches the behavior of DefaultHttpControllerSelector.
                var controllerName = GetControllerNameWithoutControllerSuffix(controllerType.Name);

                //The convention is that the innermost namespace of the controller is the version that
                //the controller supports.
                var controllerSupportedVersion = controllerType.GetInnermostNamespaceName();

                //todo: Add support for calculating the latest version of a controller

                // Create a lookup table where the key is "namespace.controller". 
                //This is to enable controllers with the same name but in different namespaces
                var controllerIdentifier = CreateControllerIdentifierKey(controllerName, controllerSupportedVersion);

                //If we've got multiple controllers with the same key we'll run into problems later on
                //when we try to select a controller. That's why we throw an error before that happens.
                if (dictionary.ContainsKey(controllerIdentifier))
                {
                    throw new Exception($"Multiple controllers with the same key is not allowed. '{controllerIdentifier}'");
                }

                dictionary[controllerIdentifier] = new HttpControllerDescriptor(_configuration, controllerType.Name, controllerType);
            }

            return dictionary;
        }

        private string GetControllerNameWithoutControllerSuffix(string controllerName)
        {
            if (controllerName.EndsWith("Controller", true, CultureInfo.InvariantCulture))
            {
                return controllerName.Remove(controllerName.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);
            }

            return controllerName;
        }

        private string CreateControllerIdentifierKey(string controllerName, string supportedVersion)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", supportedVersion, controllerName);
        }
    }

}