using ICOCore.Queries.Base;
using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Queries.Components
{
    public class HistoryBTCWithDrawQuery : BaseQuery<HistoryBTCWithDraw>
    {
        public HistoryBTCWithDrawQuery()
        {
            Status = -1;
        }
        public string Username { set; get; }
        public int Status { set; get; }
        public string Date { set; get; }

    }
}
