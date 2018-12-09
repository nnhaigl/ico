using ICOCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Dtos.Components
{
    public class ProvideHelpDto : ProvideHelp
    {
        public int PercentComplete { set; get; }
        public double EarningBIAmount { set; get; }
        public int DaysPassed { set; get; }
    }
}
