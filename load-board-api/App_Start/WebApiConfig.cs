using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using load_board_api.App_Start;

namespace load_board_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Unity
            UnityContainer container = UnityContainerBuilder.Build();
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
