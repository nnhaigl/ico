using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Dtos.Components
{
    public class UserInfoDto : UserInfo
    {
        public string Password { set; get; }
        public string ConfirmPassword { set; get; }
        public string DOB { set; get; }
        public string GenderTemp { set; get; }
        public string Recaptcha { set; get; }

    }
}
