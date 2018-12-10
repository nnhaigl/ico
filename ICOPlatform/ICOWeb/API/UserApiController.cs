using ICOCore.Dtos.Components;
using ICOCore.Messages.Base;
using ICOCore.Messages.Requests;
using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.ViewModels;
using ICOServices.Implementations;
using ICOWebCore.APIs;
using System.Web.Http;

namespace ICOWeb.API
{
    [RoutePrefix("bizzApi/user")]
    public class UserApiController : BaseFrontAPIController
    {
        private UserService _userService;
        private AccountService _accountService;

        public UserApiController()
        {
            _userService = new UserService();
            _accountService = new AccountService();
        }


        [HttpGet]
        [Route("Detail")]
        public BaseSingleResponse<UserDetailVM> GetUserDetail()
        {
            return _userService.GetUserDetail(UserHeader());
        }

        [HttpPost]
        [Route("Register")]
        public BaseSingleResponse<bool> Register(UserInfoDto regisInfo)
        {
            return _userService.Register(regisInfo);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public BaseResponse ChangePassword(PasswordRequest request)
        {
            request.Username = UserHeader();
            return _accountService.ChangePassword(request);
        }

        [HttpPost]
        [Route("ChangePassword2FA")]
        public BaseResponse ChangePassword2FA(PasswordRequest request)
        {
            request.Username = UserHeader();
            return _accountService.ChangePassword2FA(request);
        }

        [HttpPost]
        [Route("EditBasicInfo")]

        public BaseResponse EditBasicInfo(UserInfoDto dto)
        {
            dto.Username = UserHeader();
            return _userService.EditBasicInfo(dto);
        }

        [HttpPost]
        [Route("ReferralUsers")]

        public BaseListResponse<UserInfo> GetReferralUsers(UserInfoQuery query)
        {
            query.ReferralUsername = UserHeader();
            return _userService.GetReferralUsers(query);
        }

        [HttpPost]
        [Route("PlaceUser")]

        public BaseResponse PlaceUser(UserPlacementRequest query)
        {
            query.Username = UserHeader();
            return _userService.PlaceUser(query);
        }

        [HttpGet]
        [Route("Dashboard/Me")]
        public BaseSingleResponse<UserDashboardVM> GetDashboardInfo()
        {
            return _userService.GetDashboardInfo(UserHeader());
        }

    }
}