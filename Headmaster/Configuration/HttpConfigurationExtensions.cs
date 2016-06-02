using System.Web.Http;

namespace Headmaster.Configuration
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration EnableHeaderVersioning(this HttpConfiguration configuration, HeaderVersioningOptions options)
        {
            //Add our own ControllerSelector here

            return configuration;
        }
    }
}