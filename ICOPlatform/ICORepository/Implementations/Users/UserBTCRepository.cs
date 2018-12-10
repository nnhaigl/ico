using ICOCore.Repositories;
using System.Linq;
using ICOCore.Repositories.Base;

namespace ICORepository.Implementations.Users
{
    public class UserBTCRepository : Repository<UserBTC>
    {
        public UserBTC GetBTCWalletByUsername(string username)
        {
            return GetTable().FirstOrDefault(x => x.Username == username);
        }
    }
}
