using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
