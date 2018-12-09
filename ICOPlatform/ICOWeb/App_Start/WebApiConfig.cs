using System.Web.Http;

namespace ICOWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //config.Filters.Add(new BizzWebCore.Filters.WebAPIExceptionFilterAttribute());

            // Web API configuration and services

            /*
             1. if your routes in startup is registered with routes.MapRoute( 
                you must decorate your post methods with [System.Web.Mvc.HttpPost]

            2. If your routes in startup is registered with Routes.MapHttpRoute(
               you must decorate your post methods with [System.Web.Http.HttpPost]

            3. if you use MapRoute() with [System.Web.Http.HttpPost] it wont work

            4. if you use MapHttpRoute() with [System.Web.Mvc.HttpPost] it wont work
             */

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