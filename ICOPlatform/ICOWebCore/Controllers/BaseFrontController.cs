using System;
using System.Web.Mvc;
using ICOWebCore.Filters;
using ICOCore.Extensions;
using System.Web.Routing;

namespace ICOWebCore.Controllers
{
    [BaseFrontAuthenFilterAttribute]
    public abstract class BaseFrontController : Controller
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseFrontController()
        {

        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            if (exception != null)
                _logger.Error(exception);

            RedirectToRouteResult actionResult = new RedirectToRouteResult(
                                                   new RouteValueDictionary {
                                                                  { "Controller", "Global" },
                                                                  { "Action", "InternalServerError_500"}});
            filterContext.Result = actionResult;
        }
    }
}
