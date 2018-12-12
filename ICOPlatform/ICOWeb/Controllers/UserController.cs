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
using ICOCore.Repositories;

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

        public ActionResult Register(string id) // id = username người giới thiệu
        {
            string username = id;
            UserService userInfoService = new UserService();
            if (!userInfoService.IsUsernameExisted(username))
                return Redirect("/");

            ViewBag.IntroduceUsername = username;
            // logout
            ApplicationContext.Logout();
            return View();
        }


        public ActionResult CompleteRegistration(string id, string checksum) //id = username
        {
            try
            {
                string username = id;
                string hasedInfo = checksum;
                if (!string.IsNullOrWhiteSpace(hasedInfo))
                {
                    hasedInfo = hasedInfo.Replace(" ", "+");
                }
                int result = 0; // invalid
                string message = string.Empty;

                UserService _service = new UserService();
                Account acc = _service.GetAccount(username);

                if (acc.HashKey != null && acc.HashKey.Equals(hasedInfo))
                {
                    DateTime now = DateTime.Now;
                    if ((now - acc.RequestConfirmDate).TotalMinutes > CommonConstants.REGISTER_VALID_WITHIN_MINUTES)
                    {
                        // hết hạn => gửi mail lại
                        result = 1;
                        _service.ReSendRegisMail(username);
                    }
                    else
                    {
                        // complete regis
                        _service.CompleteRegistration(username);
                        return RedirectToAction("Login", "User");
                    }
                }

                ViewBag.Result = result;
                ViewBag.Username = username;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return RedirectToAction("InternalServerError", "Global");
            }

            return View();
        }

    }
}
