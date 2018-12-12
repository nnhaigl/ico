using ICOCore.Messages.Base;
using ICOCore.Repositories;
using ICOServices.Implementations;
using ICOWebCore.APIs;
using System.Web.Http;

namespace ICOWeb.API
{
    [RoutePrefix("bizzApi/common")]
    public class CommonApiController : BaseFrontAPIController
    {

        public CommonApiController()
        {
        }

        [HttpGet]
        [Route("Countries")]
        public BaseListResponse<Country> GetAllCountires()
        {
            return new CountryService().BuildDropDown();
        }

        [HttpGet]
        [Route("PHPolicies")]
        public BaseListResponse<CfgProvideHelpPolicy> GetCfgProvideHelpPolicy()
        {
            return new CfgProvideHelpPolicyService().GetAllEnablePackageDropdown();
        }

    }
}
