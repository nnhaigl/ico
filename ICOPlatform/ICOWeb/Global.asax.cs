using ICOWeb.Controllers;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ICOWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();

        //    RouteData routeData = new RouteData();
        //    routeData.Values.Add("controller", "Global");

        //    if ((Context.Server.GetLastError() is HttpException) && ((Context.Server.GetLastError() as HttpException).GetHttpCode() == 500))
        //    {
        //    }

        //    if ((Context.Server.GetLastError() is HttpException) && ((Context.Server.GetLastError() as HttpException).GetHttpCode() != 404))
        //    {
        //        routeData.Values["action"] = "InternalServerError";
        //    }
        //    else
        //    {
        //        // Handle 404 error and response code
        //        Response.StatusCode = 404;
        //        routeData.Values["action"] = "NotFound";
        //    }

        //    Response.TrySkipIisCustomErrors = true; // neu la IIS7 thi phai them dong`nay
        //    IController controller = new GlobalController();
        //    HttpContextWrapper wrapper = new HttpContextWrapper(Context);
        //    var rc = new RequestContext(wrapper, routeData);
        //    controller.Execute(rc);
        //    Response.End();
        //}
    }
}
