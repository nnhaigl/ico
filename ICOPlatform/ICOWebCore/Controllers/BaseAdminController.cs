using ICOCore.Extensions;
using ICOWebCore.Filters;
using System.Web.Mvc;

namespace ICOWebCore.Controllers
{
    [BaseAdminAuthenFilterAttribute]
    public abstract class BaseAdminController : Controller
    {
        public BaseAdminController()
        {

        }


        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}
