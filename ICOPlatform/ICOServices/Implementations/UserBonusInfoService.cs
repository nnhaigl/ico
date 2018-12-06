using ICOCore.Services.Base;
using ICORepository.Implementations.Bonous;

namespace ICOServices.Implementations
{
    class UserBonusInfoService : BaseService
    {
        private UserBonusInfoRepository _repository;
        private LogMatchingCommissionService _logMatchingCommissionService;
        public UserBonusInfoService()
        {
            _repository = new UserBonusInfoRepository();
            _logMatchingCommissionService = new LogMatchingCommissionService();
        }
    }
}
