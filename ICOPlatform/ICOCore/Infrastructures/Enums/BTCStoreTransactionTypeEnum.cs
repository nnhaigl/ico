using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Infrastructures.Enums
{
    public enum BTCStoreTransactionTypeEnum : byte
    {
        /// <summary>
        /// Người dùng nạp BTC vào hệ thống
        /// </summary>
        DEPOSIT = 0,
        /// <summary>
        /// Hệ thống chuyển BTC cho người dùng
        /// </summary>
        WITHDRAW = 1
    }
}
