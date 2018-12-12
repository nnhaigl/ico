using ICOCore.Messages.Base;

namespace ICOCore.Messages.Requests
{
    public class DepositBTCRequest : BaseRequest
    {
        public double Amount { set; get; }
        public string AddressForReceive { set; get; }
    }
}
