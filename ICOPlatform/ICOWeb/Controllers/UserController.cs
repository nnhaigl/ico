using System;
using System.Web;
using System.Web.Mvc;
using ICOWebCore.Context;
using ICOCore.Infrastructures.Constants;
using ICOServices.Implementations;
using ICOCore.Infrastructures.Enums;
using System.Net;
using Newtonsoft.Json;
using ICOCore.Entities.Extra;
using System.Web.Security;

namespace ICOWeb.Controllers
{
    public class UserController : Controller
    {
        private static readonly log4net.ILog _logger =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AccountService _accountService = new AccountService();
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

        [HttpPost]
        public JsonResult Login(string username, string password, string remember_me, string returnUrl)
        {

            TempData[ApplicationConstant.Parameter.RETURN_URL] = returnUrl;
            string captchaErrorMess = string.Empty;

            bool result = false;

            bool isOnlyAcceptAdmin = CommonConstants.IS_ONLY_ACCEPT_ADMIN;
            if (isOnlyAcceptAdmin)
            {
                if (username != "admin")
                {
                    return Json(new { IsSuccess = result, CaptchaErrorMess = captchaErrorMess });
                }
            }

            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    result = false;
                }

                ICOCore.Repositories.UserInfo user = _accountService.Login(username, password);
                if (null == user || user.ReferralStatus == (int)UserReferralStatusEnum.INACTIVE)
                {
                    ApplicationContext.IncreaseLoginFailCount();
                    result = false;
                }
                else
                {

                    ViewBag.Success = true;
                    // save cookie
                    if (remember_me == "on")
                    {
                        FormsAuthentication.SetAuthCookie(username, true);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                    }

                    result = true;

                    //Lưu Application Context
                    var userLevelService = new UserLevelService();
                    user.LevelName = userLevelService.GetByCode(user.LevelCode).Name;
                    ApplicationContext.CurrentUser = user;
                    var accountService = new AccountService();
                    ApplicationContext.CurrentAccount = accountService.GetAccount(username);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return Json(new { IsSuccess = result });
        }

    }
}
