using ICOWebCore.Controllers;
using System.Web.Mvc;

namespace ICOWeb.Areas.Admin.Controllers
{
    public class ReportController : BaseAdminController
    {
        public ActionResult GeneralByDay()
        {
            return View();
        }
    }
}