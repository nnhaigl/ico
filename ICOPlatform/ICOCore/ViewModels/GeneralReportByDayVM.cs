using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.ViewModels
{
    /// <summary>
    /// Báo cáo tổng hợp theo ngày
    /// </summary>
    public class GeneralReportByDayVM
    {
        /// <summary>
        /// Ngày báo cáo
        /// </summary>
        public DateTime Date { set; get; }
        /// <summary>
        /// Tổng nạp BTC
        /// </summary>
        public double TotalDepositBTC { set; get; }
        /// <summary>
        /// Tổng rút BTC
        /// </summary>
        public double TotalWithdrawBTC { set; get; }
        /// <summary>
        /// Tổng BTC PH
        /// </summary>
        public double TotalAmountPH { set; get; }
        /// <summary>
        /// Tổng số lượng Member mới
        /// </summary>
        public int TotalNewMember { set; get; }
        /// <summary>
        /// Tổng lượng Hoa hồng trực tiếp
        /// </summary>
        public double TotalDirectCom { set; get; }
        /// <summary>
        /// Tổng lượng hoa hồng cân cặp
        /// </summary>
        public double TotalMatchingCom { set; get; }
        /// <summary>
        /// Tổng lượng hoa hồng leader
        /// </summary>
        public double TotalLeaderCom { set; get; }
        /// <summary>
        /// Tổng số lượng Token mà user đã dùng (cho PH/GH)
        /// </summary>
        public int TotalUsedToken { set; get; }
        /// <summary>
        /// Tổng số lượng Token mà user đã mua
        /// </summary>
        public int TotalBuyToken { set; get; }
        /// <summary>
        /// Tổng số lượng Token hiện tại trong tài khoản của member
        /// </summary>
        public int TotalTokenLeftOfMember { set; get; }
        /// <summary>
        /// Tổng lượng GH
        /// </summary>
        public double TotalAmountGH { set; get; }
    }
}
