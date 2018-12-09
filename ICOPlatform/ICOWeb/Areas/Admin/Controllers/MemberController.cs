using ICOWebCore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICOWeb.Areas.Admin.Controllers
{
    public class MemberController : BaseAdminController
    {
        // GET: Admin/MemberController
        public ActionResult Index()
        {
            return View();
        }
    }
}