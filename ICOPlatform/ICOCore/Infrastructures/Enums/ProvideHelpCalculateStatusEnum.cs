using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Infrastructures.Enums
{
    public enum ProvideHelpCalculateStatusEnum : int
    {
        /// <summary>
        /// Chưa tính lãi PH ngày hiện tại
        /// </summary>
        NOT_CALCULATE = 0,
        /// <summary>
        /// Đã tính lãi PH ngày hiện tại
        /// </summary>
        CALCULATED = 1
    }
}
