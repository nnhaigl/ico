using ICOCore.Infrastructures.Enums;
using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using ICOCore.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICORepository.Implementations.Investment
{
    public class ProvideHelpRepository : Repository<ProvideHelp>
    {

        public List<ProvideHelp> Search(ProvideHelpQuery query, out int count)
        {
            var limit = query.PageSize;
            var start = (query.PageIndex - 1) * limit;

            IQueryable<ProvideHelp> linqQuery = this.Table();

            if (!string.IsNullOrWhiteSpace(query.Username))
                linqQuery = linqQuery.Where(x => x.Username == query.Username);

            if (query.Status != -1)
                linqQuery = linqQuery.Where(x => x.Status == query.Status);

            if (!string.IsNullOrWhiteSpace(query.FromDate))
            {
                DateTime fromDate = DateTime.Now;
                bool isDate = CommonUtils.ToDate(query.FromDate, out fromDate);
                if (isDate)
                    linqQuery = linqQuery.Where(x => x.CreateDate.Date.CompareTo(fromDate.Date) >= 0);
            }


            if (!string.IsNullOrWhiteSpace(query.ToDate))
            {
                DateTime toDate = DateTime.Now;
                bool isDate = CommonUtils.ToDate(query.ToDate, out toDate);
                if (isDate)
                    linqQuery = linqQuery.Where(x => x.CreateDate.Date.CompareTo(toDate.Date) <= 0);
            }

            count = linqQuery.Count();

            linqQuery = linqQuery.OrderByDescending(x => x.CreateDate).ThenBy(x => x.CreateDate.TimeOfDay);

            linqQuery = linqQuery.Skip(start).Take(limit);
            return linqQuery.ToList();
        }

        /// <summary>
        /// Lấy tất cả PH đang running của user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<ProvideHelp> AllRunningPH(string username)
        {
            return this.Table().Where(x => x.Username == username && x.Status == (int)ProvideHelpStatusEnum.RUNNING).ToList();
        }

        public double TotalAmountRunningPH(string username)
        {
            double totalRunningPHAmount = 0;
            List<ProvideHelp> runningPHs = AllRunningPH(username);
            if (runningPHs != null && runningPHs.Any())
            {
                totalRunningPHAmount = runningPHs.Sum(x => x.BTCAmount);
                totalRunningPHAmount = CommonUtils.FoatBTCAmount(totalRunningPHAmount);
            }
            return totalRunningPHAmount;
        }

        public int CountAllRunningPH()
        {
            return this.Table().Where(x => x.Status == (int)ProvideHelpStatusEnum.RUNNING).Count();
        }

        public int CountAllEndPH()
        {
            return this.Table().Where(x => x.Status == (int)ProvideHelpStatusEnum.END).Count();
        }

        public double TotalPHRunningAmount()
        {
            double totalRunningPHAmount = 0;
            List<double> phRunnings = this.GetTable().Where(x => x.Status == (int)ProvideHelpStatusEnum.RUNNING).Select(x => x.BTCAmount).ToList();
            if (phRunnings != null && phRunnings.Any())
            {
                totalRunningPHAmount = phRunnings.Sum(x => x);
                totalRunningPHAmount = CommonUtils.FoatBTCAmount(totalRunningPHAmount);
            }
            return totalRunningPHAmount;
        }

        /// <summary>
        /// Tổng số PH có thể nhận commission 
        /// => PH dang running + Số ngày đầu tư >= 10
        /// </summary>
        /// <returns></returns>
        public int TotalPHAbleToGetCommission()
        {
            DateTime now = DateTime.Now;
            return this.GetTable().Where(x => x.Status == (int)ProvideHelpStatusEnum.RUNNING // đang running
                                           && (int)(now.Date - x.CreateDate.Date).TotalDays >= 10) // đã đầu tư được 10 ngày
                                           .Count();
        }

        /// <summary>
        /// Lấy tổng số PH đang running và đã đầu tư đc 10 ngày + chưa đc tính lãi hôm nay
        /// </summary>
        /// <returns></returns>
        public int TotalPHToCalculate()
        {
            DateTime now = DateTime.Now;
            return this.GetTable().Where(x => x.Status == (int)ProvideHelpStatusEnum.RUNNING // đang running
                                           && (int)(now.Date - x.CreateDate.Date).TotalDays >= 10 // đã đầu tư được 10 ngày
                                           && x.CalculateStatus == (int)ProvideHelpCalculateStatusEnum.NOT_CALCULATE) // chưa tính lãi 
                                           .Count();
        }

    }
}
