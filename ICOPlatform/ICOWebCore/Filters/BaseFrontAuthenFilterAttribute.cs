using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System;
using ICOWebCore.Context;

namespace ICOWebCore.Filters
{
    class BaseFrontAuthenFilterAttribute : FilterAttribute, IAuthorizationFilter
    {

        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string controller = string.Empty;
        private string action = string.Empty;
        private bool isPostMethod = false;
        private bool isAjaxRequest = false;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                isPostMethod = filterContext.HttpContext.Request.HttpMethod == "POST";
                // nếu là Ajax request thì Header của HttpReqeust sẽ có key là X-Requested-With
                // với value là XMLHttpRequest 
                if (filterContext.HttpContext.Request.IsAjaxRequest() || (filterContext.Result is JsonResult))
                {
                    isAjaxRequest = true;
                }

                var viewData = filterContext.RequestContext.RouteData;

                if (viewData.Values["Controller"] != null)
                    controller = viewData.Values["Controller"].ToString().ToLower();

                if (viewData.Values["Action"] != null)
                    action = viewData.Values["Action"].ToString().ToLower();


                //if (controller == "user")
                //{
                //    if (action == "dashboard")
                //    {
                //        return;
                //    }
                //}

                var user = ApplicationContext.CurrentUser;
                if (user == null) // chưa login
                {
                    // mặc dù chưa login nhưng xóa tất cả session (nếu có)
                    ApplicationContext.Logout();
                    //RedirectToRouteResult actionResult = new RedirectToRouteResult(
                    //                             new RouteValueDictionary {
                    //                                              { "Controller", "User" },
                    //                                              { "Action", "login"}});
                    //filterContext.Result = actionResult;

                    string returnUrl = string.Empty;
                    try
                    {
                        returnUrl = HttpUtility.UrlDecode(filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery);
                    }
                    catch { }

                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                    redirectTargetDictionary.Add("action", "login");
                    redirectTargetDictionary.Add("controller", "User");
                    redirectTargetDictionary.Add(ApplicationConstant.Parameter.RETURN_URL, returnUrl);
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);

                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                throw;
            }
        }
    }
}
