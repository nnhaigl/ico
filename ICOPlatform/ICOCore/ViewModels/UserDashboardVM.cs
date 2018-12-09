using ICOCore.Repositories;

namespace ICOCore.ViewModels
{
    public class UserDashboardVM
    {
        public double TotalPHRunningAmount { set; get; }
        public double GHBalance { set; get; }
        public double TotalCoinWallet { set; get; }
        public double TotalBonus { set; get; }
        public UserInfo Me { set; get; }

    }
}
