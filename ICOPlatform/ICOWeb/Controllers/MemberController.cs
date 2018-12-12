using ICOWebCore.Controllers;
using System.Web.Mvc;

namespace ICOWeb.Controllers
{
    public class MemberController : BaseFrontController
    {
        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult Placement()
        {
            return View();
        }

        public ActionResult ReferralLink()
        {
            return View();
        }
    }
}
