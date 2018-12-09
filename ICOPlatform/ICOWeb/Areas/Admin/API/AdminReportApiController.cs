using ICOCore.Messages.Base;
using ICOCore.Queries.Components;
using ICOServices.Implementations;
using ICOWebCore.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICOWeb.Areas.Admin.API
{
    [RoutePrefix("adminApi/report")]
    public class AdminReportApiController : BaseAdminAPIController
    {
        private ReportService _reportService;

        public AdminReportApiController()
        {
            _reportService = new ReportService();
        }

        [HttpPost]
        [Route("generalByDay")]
        public BaseResponse Search(GeneralReportQuery query)
        {
            return _reportService.GetGeneralReportByDay(query);
        }

        [HttpPost]
        [Route("today")]
        public BaseResponse GetGeneralReportToday()
        {
            return _reportService.GetGeneralReportToday();
        }

        [HttpPost]
        [Route("total")]
        public BaseResponse TotalReport()
        {
            return _reportService.TotalReport();
        }

    }
}
