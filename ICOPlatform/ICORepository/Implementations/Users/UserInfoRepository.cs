using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICORepository.Implementations.Users
{
    public class UserInfoRepository : Repository<UserInfo>
    {

        public UserInfo GetByUsername(string username)
        {
            return GetTable().Where(x => x.Username == username).FirstOrDefault();
        }

        public bool IsUsernameExisted(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            return GetTable().Count(x => x.Username == username) > 0;
        }

        public bool IsUsernameExisted(int userId, string username)
        {
            if (!IsUsernameExisted(username)) return false;

            if (userId <= 0) return true;
            UserInfo user = Get(userId);

            if (user == null) return false;
            return !(user.Username == username);
        }

        public bool IsEmailExisted(string email)
        {
            return GetTable().Count(x => x.Email == email) > 0;
        }

        public bool IsPIDExisted(string pid)
        {
            return GetTable().Count(x => x.PID == pid) > 0;
        }

        public UserInfo GetChildUserId()
        {
            UserInfo usi = new UserInfo();

            return usi;
        }

        public List<UserInfo> GetReferralUsers(UserInfoQuery query, out int count)
        {
            var limit = query.PageSize;
            var start = (query.PageIndex - 1) * limit;

            IQueryable<UserInfo> linqQuery = Table();

            linqQuery = linqQuery.Where(x => x.ReferralUsername == query.ReferralUsername);

            count = linqQuery.Count();

            linqQuery = linqQuery.Skip(start).Take(limit);
            return linqQuery.ToList();
        }


    }
}
