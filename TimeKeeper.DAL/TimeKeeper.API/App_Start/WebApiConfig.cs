using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TimeKeeper.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "Pagination"));

            config.Routes.MapHttpRoute(
                name: "PagingApi",
                routeTemplate: "api/{controller}/page/{page}",
                defaults: new { page = RouteParameter.Optional, action="Get" }
            );
            /*
            config.Routes.MapHttpRoute(
               name: "ReportApi",
               routeTemplate: "api/calendar/{controller}/{id}/{year}/{month}",
               defaults: new { year = RouteParameter.Optional, month = RouteParameter.Optional }
           );
          */
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
        

            var json = GlobalConfiguration.Configuration;
            json.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            json.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            //json.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            //json.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            json.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();



        }
    }
}
