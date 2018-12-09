using ICOCore.Repositories;

namespace ICOCore.Dtos.Components
{
    public class CfgTokenDemandDto : CfgTokenDemand
    {
        public string FromAmountTemp { set; get; }
        public string ToAmountTemp { set; get; }
        public string NumberTokenTemp { set; get; }

    }
}
