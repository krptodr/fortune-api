using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using load_board_api.App_Start;
using System.Web.Http.Cors;
using System.Configuration;

namespace load_board_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Unity
            UnityContainer container = UnityContainerBuilder.Build();
            config.DependencyResolver = new UnityResolver(container);

            // CORS
            EnableCorsAttribute corsAttr = new EnableCorsAttribute(ConfigurationManager.AppSettings["CORS_ALLOWED_ORIGINS"], "*", "*");
            config.EnableCors(corsAttr);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
