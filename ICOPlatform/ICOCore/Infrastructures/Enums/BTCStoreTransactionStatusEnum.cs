using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Infrastructures.Enums
{
    public enum BTCStoreTransactionStatusEnum : byte
    {
        NOT_CONFIRMED = 1,
        CONFIRMED = 2,
        REJECTED = 3
    }
}
