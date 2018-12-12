using System;
using System.Web;

namespace ICOWebCore.Utils
{
    public class WebCoreUtils
    {
        public static string GetDisplayNumberRecord(int pageIndex, int pageSize, int totalRecord)
        {
            pageIndex = pageIndex - 1;
            int startRecordPosition = (pageIndex * pageSize) + 1;
            int endRecordPosition = startRecordPosition + (pageSize - 1);

            if (endRecordPosition > totalRecord)
                endRecordPosition = totalRecord;

            return string.Format("Hiển thị bản ghi thứ {0} đến {1} trên tổng số {2} bản ghi", startRecordPosition, endRecordPosition, totalRecord);
        }

        public static int TotalPageCount(int totalRecord, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalRecord / pageSize);
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            return baseUrl;
        }
    }
}
