using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Infrastructures.Enums
{
    public enum HistoryBTCWithDrawStatusEnum
    {
        /// <summary>
        /// Chờ confirm
        /// </summary>
        WAITING_CONFIRM = 0,
        /// <summary>
        /// Đã confirm và chờ giao dịch trên Blockchain khi có 1 xác thực
        /// </summary>
        CONFIRMED_AND_WAITING_BLOCKCHAIN = 1,
        /// <summary>
        /// Confirm withdraw thành công khi có giao dich trên Blockchain
        /// </summary>
        CONFIRMED_WITHDRAW_OK = 2,
        /// <summary>
        /// Reject 
        /// </summary>
        REJECTED = 3
    }
}
