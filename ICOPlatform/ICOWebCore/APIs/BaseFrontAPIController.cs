using ICOWebCore.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ICOWebCore.APIs
{
    [WebAPIExceptionFilterAttribute]
    public class BaseFrontAPIController : ApiController
    {
        //private HttpControllerContext _controllerContext;
        private const string HEADER_USER_REQUEST = "Username";
        public BaseFrontAPIController()
        {

        }

        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    this._controllerContext = controllerContext;
        //}

        protected string GetHeader(string headerName)
        {
            IEnumerable<string> headerValues;
            var headerValue = string.Empty;
            var keyFound = Request.Headers.TryGetValues(headerName, out headerValues);
            if (keyFound)
            {
                headerValue = headerValues.FirstOrDefault();
            }
            return headerValue;
        }

        protected string UserHeader()
        {
            return GetHeader(HEADER_USER_REQUEST);
        }
    }
}
