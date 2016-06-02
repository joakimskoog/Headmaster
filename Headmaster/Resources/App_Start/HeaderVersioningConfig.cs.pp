using System.Web.Http;
using WebActivatorEx;
using $rootnamespace$;
using Headmaster.Configuration;

[assembly: PreApplicationStartMethod(typeof(HeaderVersioningConfig), "Register")]

namespace $rootnamespace$
{
    public class HeaderVersioningConfig
    {
		public static void Register() 
		{
			GlobalConfiguration.Configuration.EnableHeaderVersioning(new HeaderVersioningOptions("mediaTypeHere", "mediaTypeParameterHere"));
		}
    }
}
