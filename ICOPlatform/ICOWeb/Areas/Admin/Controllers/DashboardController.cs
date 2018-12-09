using ICOWebCore.Controllers;
using System.Web.Mvc;

namespace ICOWeb.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        //
        // GET: /Admin/Dashboard/
        public ActionResult Index()
        {
            return View();
        }
    }
}
