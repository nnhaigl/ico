namespace ICOWebCore.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    /// <summary>
    /// Xử lý exception với WebAPI
    /// </summary>
    public class WebAPIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly log4net.ILog _logger =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(HttpActionExecutedContext context)
        {
            try
            {
                if (context.Exception != null)
                    _logger.Error(context.Exception);
            }
            catch { }

            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
        }
    }
}
