using ICOWebCore.APIs;
using System.Web.Http;
using ICOServices.Implementations;
using ICOCore.Messages.Base;
using ICOCore.Messages.Requests;
using ICOCore.ViewModels;
using ICOCore.Repositories;

namespace ICOWeb.Areas.Admin.API
{
    [RoutePrefix("adminApi/user")]
    public class AdminUserApiController : BaseAdminAPIController
    {
        private UserService _service;
        private AccountService _accountService;

        public AdminUserApiController()
        {
            _service = new UserService();
            _accountService = new AccountService();
        }

        [HttpGet]
        [Route("Detail/{username}")]
        public BaseSingleResponse<UserInfo> GetUserDetail(string username)
        {
            return _service.GetUserInfoAPI(username);
        }

        [HttpPost]
        [Route("editBasicInfo/{username}")]
        public BaseResponse AdminEditBasicInfo(UserInfo userInfo)
        {
            return _service.AdminEditBasicInfo(userInfo);
        }

        [HttpPost]
        [Route("verified/{username}")]
        public BaseResponse AdminVerifiedUser(string username)
        {
            return _service.AdminVerifiedUser(username);
        }

        [HttpPost]
        [Route("notverified/{username}")]
        public BaseResponse AdminNotVerifiedUser(string username)
        {
            return _service.AdminNotVerifiedUser(username);
        }

        [HttpPost]
        [Route("block/{username}")]
        public BaseResponse AdminBlockMember(string username)
        {
            return _service.AdminBlockMember(username);
        }

        [HttpPost]
        [Route("unblock/{username}")]
        public BaseResponse AdminUnBlockMember(string username)
        {
            return _service.AdminUnBlockMember(username);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public BaseResponse ChangePassword(PasswordRequest request)
        {
            return _accountService.ChangePasswordForMember(request);
        }

        [HttpPost]
        [Route("ChangePassword2FA")]
        public BaseResponse ChangePassword2FA(PasswordRequest request)
        {
            request.Username = UserHeader();
            return _accountService.ChangePassword2FAForMember(request);
        }

        [HttpPost]
        [Route("SetLevel")]
        public BaseResponse SetUserLevel([FromBody]dynamic value)
        {
            string username = value.Username.Value;
            string levelCode = value.LevelCode.Value;
            return _service.SetLevel(username, levelCode);
        }


        [HttpGet]
        [Route("UserLevel")]
        public BaseResponse GetAllUserLevel()
        {
            return new UserLevelService().GetAllAPI();
        }

    }
}