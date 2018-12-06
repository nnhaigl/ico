using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Net;
using System;
using ICOWebCore.Context;

namespace ICOWebCore.Filters
{
    public class BaseAdminAuthenFilterAttribute : FilterAttribute, IAuthorizationFilter
    {

        private const string _AREA_NAME = "Global";

        private string area = string.Empty;
        private string controller = string.Empty;
        private string action = string.Empty;
        private bool isPostMethod = false;
        private bool isAjaxRequest = false;

        public void OnAuthorization(AuthorizationContext filterContext) // ham nay se duoc chay bat cu o trang nao
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

                var user = ApplicationContext.CurrentUser;
                var acc = ApplicationContext.CurrentAccount;

                if (user == null || acc == null || !acc.IsSuper.HasValue || !acc.IsSuper.Value) // chưa login Hoặc không phải super admin
                {
                    // mặc dù chưa login nhưng xóa tất cả session (nếu có)
                    ApplicationContext.Logout();

                    string returnUrl = string.Empty;
                    try
                    {
                        returnUrl = HttpUtility.UrlDecode(filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery);
                    }
                    catch { }

                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                    redirectTargetDictionary.Add("action", "login");
                    redirectTargetDictionary.Add("controller", "User");
                    redirectTargetDictionary.Add("Area", "");
                    redirectTargetDictionary.Add(ApplicationConstant.Parameter.RETURN_URL, returnUrl);
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}