using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
// casing issues
namespace AuctionTracker
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

			var config = GlobalConfiguration.Configuration;

			// Replace the default JsonFormatter with our custom one
			var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

		}
	}
}
