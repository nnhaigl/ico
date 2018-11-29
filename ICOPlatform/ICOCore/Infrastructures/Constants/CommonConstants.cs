using System.Configuration;

namespace ICOCore.Infrastructures.Constants
{ 
    public class CommonConstants
    {
        public static bool IS_IN_PRODUCTION = false;
        public static bool IS_ONLY_ACCEPT_ADMIN = false;

        static CommonConstants()
        {
            IS_ONLY_ACCEPT_ADMIN = bool.Parse(ConfigurationManager.AppSettings["onlyAcceptAdmin"].ToString());
            IS_IN_PRODUCTION = bool.Parse(ConfigurationManager.AppSettings["isInProduction"].ToString());
        }
        
    }
}
