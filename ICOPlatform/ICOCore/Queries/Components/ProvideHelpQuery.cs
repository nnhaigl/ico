using ICOCore.Queries.Base;
using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Queries.Components
{
    public class ProvideHelpQuery : BaseQuery<ProvideHelp>
    {
        public string Username { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }

    }
}
