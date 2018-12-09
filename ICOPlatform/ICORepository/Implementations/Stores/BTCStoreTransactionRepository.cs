using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using ICOCore.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICORepository.Implementations.Stores
{
    public class BTCStoreTransactionRepository : Repository<BTCStoreTransaction>
    {

        public List<BTCStoreTransaction> Search(BTCStoreTransactionQuery query, out int count)
        {
            var result = new List<BTCStoreTransaction>();

            var limit = query.PageSize;
            var start = (query.PageIndex - 1) * limit;

            IQueryable<BTCStoreTransaction> linqQuery = this.GetTable();

            if (!string.IsNullOrWhiteSpace(query.Username))
                linqQuery = linqQuery.Where(x => x.Username == query.Username);

            if (query.Type != -1)
                linqQuery = linqQuery.Where(x => x.Type == query.Type);

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
            linqQuery = linqQuery.OrderByDescending(p => p.CreatedDate);
            linqQuery = linqQuery.Skip(start).Take(limit);
            result = linqQuery.ToList();

            return result;
        }

    }
}
