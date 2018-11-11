using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOWebCore.Context
{
    class ApplicationConstant
    {
        public static class Session
        {
            public const string SESSION_USERNAME = "session_username";
            public const string SESSION_USER = "session_user";
            public const string SESSION_ACCOUNT = "session_account";
            public const string TEMPORARILY_BAN_ACCOUNT = "TemporailyBanAccount";
            public const string LOGIN_FAIL_COUNT = "LoginFailCount";
        }

        public static class Parameter
        {
            public const string RETURN_URL = "returnUrl";
        }

        public static class ContentURL
        {
            public const string FOLDER_UPLOAD_USER_AVATAR = "~/Content/upload/avatar";
        }
    }
}
