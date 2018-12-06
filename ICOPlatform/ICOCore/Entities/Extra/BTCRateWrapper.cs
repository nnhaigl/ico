using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Entities.Extra
{
    public class BTCRateWrapper
    {
        public BTCRateWrapper()
        {
        }

        public string CurrencyCode { set; get; }

        /// <summary>
        /// is the most recent market price
        /// </summary>
        public string Last { set; get; }
        public string Buy { set; get; }
        public string Sell { set; get; }
        /// <summary>
        /// is the currency symbol. 
        /// </summary>
        public string Symbol { set; get; }

    }
}
