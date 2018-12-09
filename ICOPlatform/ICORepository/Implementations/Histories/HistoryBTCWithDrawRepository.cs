using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using ICOCore.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICORepository.Implementations.Histories
{
    public class HistoryBTCWithDrawRepository : Repository<HistoryBTCWithDraw>
    {
        public List<HistoryBTCWithDraw> GetWithDrawHistoryByUsername(string username)
        {
            return GetTable().Where(x => x.Username == username).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<HistoryBTCWithDraw> AllWithDrawToday(string username)
        {
            DateTime now = DateTime.Now;
            return this.GetTable().Where(x => x.Username == username &&
                                                     x.CreatedDate.Year == now.Year &&
                                                     x.CreatedDate.Month == now.Month &&
                                                     x.CreatedDate.Day == now.Day).ToList();
        }

        public List<HistoryBTCWithDraw> Search(HistoryBTCWithDrawQuery query, out int count)
        {
            var limit = query.PageSize;
            var start = (query.PageIndex - 1) * limit;

            IQueryable<HistoryBTCWithDraw> linqQuery = this.Table();

            if (!string.IsNullOrWhiteSpace(query.Username))
                linqQuery = linqQuery.Where(x => x.Username == query.Username);

            if (query.Status != -1)
                linqQuery = linqQuery.Where(x => x.Status == query.Status);

            if (!string.IsNullOrWhiteSpace(query.Date))
            {
                DateTime date = DateTime.Now;
                bool isDate = CommonUtils.ToDate(query.Date, out date);
                if (isDate)
                    linqQuery = linqQuery.Where(x => x.CreatedDate.Year == date.Year
                                                    && x.CreatedDate.Month == date.Month
                                                    && x.CreatedDate.Day == date.Day);
            }

            count = linqQuery.Count();

            linqQuery = linqQuery.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.CreatedDate.TimeOfDay);

            linqQuery = linqQuery.Skip(start).Take(limit);
            return linqQuery.ToList();
        }

    }
}
