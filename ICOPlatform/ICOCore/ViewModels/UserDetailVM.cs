using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.ViewModels
{
    public class UserDetailVM
    {
        public UserInfo UserInfo { set; get; }
        public Account Account { set; get; }
        public Country Country { set; get; }
        public UserLevel UserLevel { set; get; }
        public UserBIMain UserBIMain { set; get; }
        public UserBTC UserBTC { set; get; }
        public UserBonusInfo UserBonusInfo { set; get; }
        public double TotalPHAmount { set; get; }
        public double TotalAmountAbleToGH { set; get; }
        public int TokenBalance { set; get; }

    }
}
