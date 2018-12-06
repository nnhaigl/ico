using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net;
using System.Web;
using ICOWebCore.Context;

namespace ICOWebCore.Filters
{
    public static class ResponseHandler
    {


        public static void DoResponse(ControllerContext context, HttpStatusCode httpStatusCode, bool isAjaxRequest, string area)
        {
            if (isAjaxRequest)
                JsonResult(context, httpStatusCode, area);
            else
                Redirect(context, httpStatusCode, area);
        }

        /// <summary>
        /// Xử lý khi request tới Action có return type là Action Result
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="httpStatusCode"></param>
        private static void Redirect(ControllerContext context, HttpStatusCode httpStatusCode, string area /*ControllerContext context*/)
        {
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;
            RedirectToRouteResult actionResult = null;

            switch (httpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        ApplicationContext.Logout();
                        actionResult = new RedirectToRouteResult(
                                                   new RouteValueDictionary { { "Area", area },
                                                                  { "Controller", "Authentication" },
                                                                  { "Action", "login"}});
                        break;
                    }

                case HttpStatusCode.Forbidden:
                    {
                        actionResult = new RedirectToRouteResult(
                                                   new RouteValueDictionary {{ "Area", area },
                                                                    { "Controller", "Redirect" },
                                                                    { "Action", "AccessDenied"}});
                    }
                    break;

                case HttpStatusCode.InternalServerError:
                    {
                        actionResult = new RedirectToRouteResult(
                                                 new RouteValueDictionary {{ "Area", area },
                                                                    { "Controller", "Redirect" },
                                                                    { "Action", "InternalServerError"}});
                    }
                    break;

                case HttpStatusCode.NotFound:
                    {
                        actionResult = new RedirectToRouteResult(
                                                new RouteValueDictionary {{ "Area", area },
                                                                    { "Controller", "Redirect" },
                                                                    { "Action", "NotFound"}});
                    }
                    break;

                default:
                    break;
            }


            if (context is AuthorizationContext)
            {
                var authorizationContext = (AuthorizationContext)context;

                if (HttpStatusCode.Unauthorized == httpStatusCode)
                {
                    string returnUrl = authorizationContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    returnUrl = HttpUtility.UrlDecode(returnUrl);
                    actionResult.RouteValues["returnUrl"] = returnUrl;
                }
                authorizationContext.Result = actionResult;

            }
            else if (context is ExceptionContext)
            {
                var exceptionContext = (ExceptionContext)context;

                if (HttpStatusCode.Unauthorized == httpStatusCode)
                {
                    string returnUrl = exceptionContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    returnUrl = HttpUtility.UrlDecode(returnUrl);
                    actionResult.RouteValues["returnUrl"] = returnUrl;
                }
                exceptionContext.Result = actionResult;
            }
        }

        /// <summary>
        /// Xử lý khi request tới Action có return type là Json Result
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="httpStatusCode"></param>
        private static void JsonResult(ControllerContext context, HttpStatusCode httpStatusCode, string area /*ControllerContext context*/)
        {
            string returnData = string.Empty;
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            switch (httpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        ApplicationContext.Logout();
                        returnData = "Not logged in !";
                        break;
                    }

                case HttpStatusCode.Forbidden:
                    {
                        returnData = "Access Denied !";
                    }
                    break;

                case HttpStatusCode.InternalServerError:
                    {
                        returnData = "Internal Server Error !";
                    }
                    break;

                case HttpStatusCode.NotFound:
                    {
                        returnData = "Internal Server Error !";
                    }
                    break;

                default:
                    break;
            }

            if (context is ExceptionContext)
            {
                var filterContext = (ExceptionContext)context;

                filterContext.Result = new JsonResult()
                {
                    Data = returnData
                };
            }
            else if (context is AuthorizationContext)
            {
                var filterContext = (AuthorizationContext)context;

                filterContext.Result = new JsonResult()
                {
                    Data = returnData
                };
            }
        }

    }
}
