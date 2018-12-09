using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Infrastructures.Enums
{
    /// <summary>
    /// Loại giao dịch token
    /// </summary>
    public enum TransactionTokenTransferTypeEnum
    {
        /// <summary>
        /// Mua Token từ Hệ thống
        /// </summary>
        BUY_FROM_SYSTEM = 1,
        /// <summary>
        /// Trả phí token khi PH
        /// </summary>
        PAID_FOR_PROVIDE_HELP = 2,
        /// <summary>
        /// Trả phí Token khi GH
        /// </summary>
        PAID_FOR_GET_HELP = 3,
        /// <summary>
        /// Chuyển Token cho user khác trong hệ thống
        /// </summary>
        TRANSFER_TO_OTHER_USER = 4
    }
}
