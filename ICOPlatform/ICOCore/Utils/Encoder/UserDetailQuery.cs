using ICOCore.Queries.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Utils.Encoder
{
    public class UserDetailQuery : BaseQuery<UserDetailQuery>
    {
        public UserDetailQuery()
        {
            ReferralStatus = -1;
            IdentifyStatus = -1;
        }

        public string Keyword { set; get; }
        public string Country { set; get; }
        public string CreatedDate { set; get; }
        public int ReferralStatus { set; get; }
        public int IdentifyStatus { set; get; }

    }
}
