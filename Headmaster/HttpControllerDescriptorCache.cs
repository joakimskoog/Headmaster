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
        private readonly IControllerTypesResolver _controllerTypesResolver;

        private readonly Lazy<IDictionary<string, HttpControllerDescriptor>> _controllers;

        public IDictionary<string, HttpControllerDescriptor> ControllerDescriptors => _controllers.Value;
        private readonly IDictionary<string, string> _defaultControllerVersions = new Dictionary<string, string>();

        public HttpControllerDescriptorCache(HttpConfiguration configuration, IControllerTypesResolver controllerTypesResolver)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (controllerTypesResolver == null) throw new ArgumentNullException(nameof(controllerTypesResolver));
            _configuration = configuration;
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

                //Calculate the highest available version for each controller, this will be used as the default version
                var latestVersionForController = CalculateHighestVersion(controllerName, controllerSupportedVersion);
                _defaultControllerVersions[controllerName.ToLower()] = latestVersionForController;

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

        public bool TryGet(string controllerName, string version, out HttpControllerDescriptor controllerDescriptor)
        {
            if (TryGetControllerDescriptor(controllerName, version, out controllerDescriptor))
            {
                return true;
            }

            string latestVersion = "";
            if (TryGetLatestVersion(controllerName, out latestVersion))
            {
                if (TryGetControllerDescriptor(controllerName, latestVersion, out controllerDescriptor))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryGetControllerDescriptor(string controllerName, string version, out HttpControllerDescriptor controllerDescriptor)
        {
            var key = CreateControllerIdentifierKey(controllerName, version);
            if (ControllerDescriptors.TryGetValue(key, out controllerDescriptor))
            {
                return true;
            }

            return false;
        }

        private bool TryGetLatestVersion(string controllerName, out string latestVersion)
        {
            var cleanedControllerName = GetControllerNameWithoutControllerSuffix(controllerName).ToLower(CultureInfo.InvariantCulture);

            if (_defaultControllerVersions.TryGetValue(cleanedControllerName, out latestVersion))
            {
                return true;
            }

            latestVersion = string.Empty;
            return false;
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

        private string CalculateHighestVersion(string controllerName, string version)
        {
            string defaultVersion = null;
            if (_defaultControllerVersions.TryGetValue(controllerName.ToLower(CultureInfo.InvariantCulture), out defaultVersion))
            {
                var result = string.Compare(version, defaultVersion, StringComparison.OrdinalIgnoreCase);

                if (result <= 0)
                {
                    return defaultVersion;
                }
            }

            return version;
        }
    }
}