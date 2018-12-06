using System;
using System.Web;
using System.Web.Security;
using ICOCore.Repositories;
using ICOServices.Implementations;

namespace ICOWebCore.Context
{
    public class ApplicationContext
    {
        public static UserInfo CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[ApplicationConstant.Session.SESSION_USER] == null)
                {
                    string username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var _service = new AccountService();
                        var user = _service.GetByUsername(username);
                        var userLevelService = new UserLevelService();
                        user.LevelName = userLevelService.GetByCode(user.LevelCode).Name;
                        HttpContext.Current.Session[ApplicationConstant.Session.SESSION_USER] = user;
                    }
                    else
                    {
                        return null;
                    }
                }
                return (UserInfo)HttpContext.Current.Session[ApplicationConstant.Session.SESSION_USER];
            }
            set { HttpContext.Current.Session[ApplicationConstant.Session.SESSION_USER] = value; }
        }

        public static Account CurrentAccount
        {
            get
            {
                if (HttpContext.Current.Session[ApplicationConstant.Session.SESSION_ACCOUNT] == null)
                {
                    string username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var _service = new AccountService();
                        var account = _service.GetAccount(username);
                        HttpContext.Current.Session[ApplicationConstant.Session.SESSION_ACCOUNT] = account;
                    }
                    else
                    {
                        return null;
                    }
                }
                return (Account)HttpContext.Current.Session[ApplicationConstant.Session.SESSION_ACCOUNT];
            }
            set { HttpContext.Current.Session[ApplicationConstant.Session.SESSION_ACCOUNT] = value; }
        }

        /// <summary>
        /// Clear cache người dùng hiện tại
        /// </summary>
        public static void InvalidateUserContext()
        {
            HttpContext.Current.Session[ApplicationConstant.Session.SESSION_USER] = null;
        }

        public static bool IsLogged()
        {
            //return HttpContext.Current.User.Identity.IsAuthenticated;
            var user = CurrentUser;
            return user != null;
        }

        /// <summary>
        /// Username đã logged in
        /// </summary>
        public static string CurrentUsername
        {
            get
            {
                var user = CurrentUser;
                return user != null ? user.Username : string.Empty;
            }
        }

        public static string CurrentLevelName
        {
            get
            {
                var user = CurrentUser;
                return user != null ? user.LevelName : string.Empty;
            }
        }

        /// <summary>
        /// Tăng số lần login failed
        /// </summary>
        public static void IncreaseLoginFailCount()
        {
            if (HttpContext.Current.Session[ApplicationConstant.Session.LOGIN_FAIL_COUNT] == null)
            {
                HttpContext.Current.Session[ApplicationConstant.Session.LOGIN_FAIL_COUNT] = 1;
            }
            else
            {
                int fail = (int)HttpContext.Current.Session[ApplicationConstant.Session.LOGIN_FAIL_COUNT];
                fail++;

                HttpContext.Current.Session[ApplicationConstant.Session.LOGIN_FAIL_COUNT] = fail;
                if (fail == 5)
                    HttpContext.Current.Session[ApplicationConstant.Session.TEMPORARILY_BAN_ACCOUNT] = true;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            //// xóa authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);

            //// xóa session cookie với key là NET_SessionId
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie2);
        }

    }
}
