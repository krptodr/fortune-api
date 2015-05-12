using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using fortune_api.App_Start;
using System.Web.Http.Cors;
using System.Configuration;
using fortune_api.Controllers.Filters;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;

namespace fortune_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Unity
            UnityContainer container = UnityContainerBuilder.Build();
            IDependencyResolver resolver = new UnityResolver(container);
            config.DependencyResolver = resolver;

            // CORS
            EnableCorsAttribute corsAttr = new EnableCorsAttribute(ConfigurationManager.AppSettings["CORS_ALLOWED_ORIGINS"], "*", "*");
            config.EnableCors(corsAttr);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Global filters
            config.Filters.Add((ApiExceptionFilterAttribute)resolver.GetService(typeof(ApiExceptionFilterAttribute)));
            config.Filters.Add((AuthenticationFilter)resolver.GetService(typeof(AuthenticationFilter)));
            config.Filters.Add((AuthenticationRequiredAttribute)resolver.GetService(typeof(AuthenticationRequiredAttribute)));
        }
    }
}
