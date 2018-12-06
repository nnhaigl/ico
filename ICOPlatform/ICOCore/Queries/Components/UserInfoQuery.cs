using ICOCore.Queries.Base;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Queries.Components
{
    public class UserInfoQuery : BaseQuery<UserInfo>
    {
        public UserInfoQuery()
        {
            Username = null;
        }

        public string Username { set; get; }
        public string ReferralUsername { set; get; }
        public string ParentUsername { set; get; }

    }
}
