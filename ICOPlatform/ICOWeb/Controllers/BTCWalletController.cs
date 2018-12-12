using ICOWebCore.Controllers;
using System.Web.Mvc;

namespace ICOWeb.Controllers
{
    public class BTCWalletController : BaseFrontController
    {
        //
        // GET: /Token/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transfer()
        {
            return View();
        }

    }
}
