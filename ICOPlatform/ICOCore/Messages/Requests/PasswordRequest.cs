using ICOCore.Messages.Base;

namespace ICOCore.Messages.Requests
{
    public class PasswordRequest : BaseRequest
    {
        public string OldPassword { set; get; }
        public string NewPassword { set; get; }
        public string ConfirmPassword { set; get; }
        public string MemberUsername { set; get; }

    }
}
