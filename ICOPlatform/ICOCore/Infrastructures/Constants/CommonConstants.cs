using System.Configuration;

namespace ICOCore.Infrastructures.Constants
{ 
    public class CommonConstants
    {
        public const string ENCRYPT_TYPE_MD5 = "MD5";

        public const string DATE_FORMAT = "dd/MM/yyyy";

        public const string OTP_DATE_TIME_FORMAT = "ddMMyyyyHHmmss";
        public const string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";

        public const int OTP_VALID_WITHIN_MINUTES = 3;

        public const int REGISTER_VALID_WITHIN_MINUTES = 5;

        public static string SHORT_HOST_NAME = string.Empty;
        public static string FULL_HOST_NAME = string.Empty;
        public static bool IS_IN_PRODUCTION = false;
        public static bool IS_ONLY_ACCEPT_ADMIN = false;


        static CommonConstants()
        {
            IS_ONLY_ACCEPT_ADMIN = bool.Parse(ConfigurationManager.AppSettings["onlyAcceptAdmin"].ToString());
            IS_IN_PRODUCTION = bool.Parse(ConfigurationManager.AppSettings["isInProduction"].ToString());

            //SHORT_HOST_NAME = ConfigurationManager.AppSettings["short_host_name"].ToString();
            //FULL_HOST_NAME = ConfigurationManager.AppSettings["full_host_name"].ToString();
        }

    }
}
