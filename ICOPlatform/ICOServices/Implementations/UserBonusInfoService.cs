using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICOCore.Utils.Common;
using ICORepository.Implementations.Bonous;
using System.Linq;

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

        public UserBonusInfo GetByUsername1(string username, bool isCalculateLeftRight)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            isCalculateLeftRight = false; // disable function
            UserBonusInfo userBonusInfo = _repository.GetTable().FirstOrDefault(x => x.Username == username);
            if (isCalculateLeftRight)
            {
                if (userBonusInfo.TotalPHAmountDownside > (userBonusInfo.LeftSide + userBonusInfo.RightSide))
                {
                    // lấy UserInfo
                    UserService userService = new UserService();
                    UserInfo userInfo = userService.GetUserInfo(userBonusInfo.Username);

                    // lấy con bên trái
                    if (userInfo.LeftPosId != 0)
                    {
                        UserInfo left = userService.GetUserInfoById(userInfo.LeftPosId);
                        UserBonusInfo leftBonus = _repository.GetTable().Single(x => x.Username == left.Username);
                        //userBonusInfo.TotalPHAmountDownside += leftBonus.TotalPHAmountDownside;
                        //userBonusInfo.TotalPHAmountDownside = CommonUtils.FoatBTCAmount(userBonusInfo.TotalPHAmountDownside);

                        userBonusInfo.LeftSide = leftBonus.TotalPHAmountDownside;
                        userBonusInfo.LeftSide = CommonUtils.FoatBTCAmount(userBonusInfo.LeftSide);
                    }

                    // lấy con bên phải
                    if (userInfo.RightPosId != 0)
                    {
                        UserInfo right = userService.GetUserInfoById(userInfo.RightPosId);
                        UserBonusInfo rightBonus = _repository.GetTable().Single(x => x.Username == right.Username);
                        //userBonusInfo.TotalPHAmountDownside += rightBonus.TotalPHAmountDownside;
                        //userBonusInfo.TotalPHAmountDownside = CommonUtils.FoatBTCAmount(userBonusInfo.TotalPHAmountDownside);

                        userBonusInfo.RightSide = rightBonus.TotalPHAmountDownside;
                        userBonusInfo.RightSide = CommonUtils.FoatBTCAmount(userBonusInfo.RightSide);
                    }
                    userBonusInfo.TotalPHAmountDownside = 0;
                    userBonusInfo.TotalPHAmountDownside += userBonusInfo.LeftSide;
                    userBonusInfo.TotalPHAmountDownside += userBonusInfo.RightSide;
                    userBonusInfo.TotalPHAmountDownside = CommonUtils.FoatBTCAmount(userBonusInfo.TotalPHAmountDownside);
                    _repository.SubmitChanges();
                }
            }

            return userBonusInfo;
        }
    }
}
