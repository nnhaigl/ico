using ICOCore.Messages.Base;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using ICOCore.Services.Base;
using ICORepository.Implementations.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class UserLevelService : BaseService
    {

        private UserLevelRepository _repository;

        public UserLevelService()
        {
            _repository = new UserLevelRepository();
        }

        public List<UserLevel> GetAll()
        {
            return _repository.GetAll();
        }

        public UserLevel GetByCode(string code)
        {
            return _repository.GetTable().Single(x => x.Code == code);
        }

        public BaseListResponse<UserLevel> GetAllAPI()
        {
            var response = new BaseListResponse<UserLevel>();
            try
            {
                response.Data = this.GetAll();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

    }
}
