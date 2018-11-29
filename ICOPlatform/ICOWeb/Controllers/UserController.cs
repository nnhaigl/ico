using System;
using System.Web;
using System.Web.Mvc;
using ICOWebCore.Context;

namespace ICOWeb.Controllers
{
    public class UserController : Controller
    {
        private static readonly log4net.ILog _logger =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Login(string returnUrl)
        {
            TempData[ApplicationConstant.Parameter.RETURN_URL] = returnUrl;
            return View();
        }
    
        public ActionResult Logout()
        {
            ApplicationContext.Logout();
            return Redirect("/user/login");
        }

    }
}
