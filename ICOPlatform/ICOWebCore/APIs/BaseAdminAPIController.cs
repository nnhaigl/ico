using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICOWebCore.APIs
{
    public class BaseAdminAPIController : ApiController
    {
        private const string HEADER_USER_REQUEST = "Username";

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
