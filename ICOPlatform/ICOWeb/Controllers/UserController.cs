using System;
using System.Web;
using ICOCore.Entities.Extra;
using System.Web.Mvc;
using System.Net;
using System.Web.Security;
using Newtonsoft.Json;
using ICOWebCore.Context;
using ICOServices.Implementations;
using ICOCore.Infrastructures.Constants;
using ICOCore.Infrastructures.Enums;

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
        public JsonResult Login(string username, string password, string remember_me, string returnUrl, string g_recaptcha_response)
        {

            TempData[ApplicationConstant.Parameter.RETURN_URL] = returnUrl;
            string captchaErrorMess = string.Empty;

            bool result = false;
            var isCapChaSuccess = true;
            var isCaptchaErr = false;

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

                ICOCore.Repositories.Base.UserInfo user = _accountService.Login(username, password);
                if (null == user || user.ReferralStatus == (int)UserReferralStatusEnum.INACTIVE)
                {
                    ApplicationContext.IncreaseLoginFailCount();
                    result = false;
                }
                else
                {
                    // ------------------ kiểm tra captcha
                    bool isInProduction = CommonConstants.IS_IN_PRODUCTION;

                    if (isInProduction)
                    {
                        var response = g_recaptcha_response;  //Request["g-recaptcha-response"];
                        if (string.IsNullOrWhiteSpace(response))
                        {
                            captchaErrorMess = "The response parameter is missing...";
                            isCaptchaErr = true;
                        }
                        else
                        {
                            //secret that was generated in key value pair
                            const string secret = "6Ld1YgkUAAAAAIbSQSBeM2uSV7sN5dUc3AWXV4-a";

                            var client = new WebClient();
                            var reply =
                                client.DownloadString(
                                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                                secret, response));

                            var captchaResponse = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(reply);

                            //when response is false check for the error message
                            isCapChaSuccess = captchaResponse.Success;
                            if (!isCapChaSuccess)
                            {
                                isCaptchaErr = captchaResponse.ErrorCodes.Count <= 0 ? false : true;

                                if (isCaptchaErr)
                                {
                                    // nếu có lỗi capcha
                                    var error = captchaResponse.ErrorCodes[0].ToLower();
                                    switch (error)
                                    {
                                        case ("missing-input-secret"):
                                            captchaErrorMess = "The secret parameter is missing.";
                                            break;
                                        case ("invalid-input-secret"):
                                            captchaErrorMess = "The secret parameter is invalid or malformed.";
                                            break;
                                        case ("missing-input-response"):
                                            captchaErrorMess = "The response parameter is missing.";
                                            break;
                                        case ("invalid-input-response"):
                                            captchaErrorMess = "The response parameter is invalid or malformed.";
                                            break;
                                        default:
                                            captchaErrorMess = "Error occured. Please try again";
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    if ((!isInProduction) || (isInProduction && !isCaptchaErr))
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
                    }

                    // ------------------ /kiểm tra captcha
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return Json(new { IsSuccess = result, CaptchaErrorMess = captchaErrorMess });
        }

    }
}
