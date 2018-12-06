using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICORepository.Implementations.Users
{
    public class AccountRepository : Repository<Account>
    {
        public bool IsUsernameExisted(string username)
        {
            return GetTable().Count(x => x.Username == username) > 0;
        }

        public bool IsUsernameExisted(int accId, string username)
        {
            if (!IsUsernameExisted(username)) return false;

            if (accId <= 0) return true;
            Account acc = Get(accId);

            if (acc == null) return false;
            return !(acc.Username == username);
        }

    }
}
