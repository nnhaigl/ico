using ICOWebCore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICOWeb.Controllers
{
    public class MeController : BaseFrontController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }
    }
}