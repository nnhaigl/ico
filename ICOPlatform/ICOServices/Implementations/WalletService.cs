using ICOCore.Services.Base;
using ICOCore.Repositories;
using ICORepository.Implementations.Users;
using System.Linq;

namespace ICOServices.Implementations
{
    public class WalletService : BaseService
    {
        private static readonly log4net.ILog _logger =
                            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
 
        private UserBTCRepository _userBTCRepository;
        private UserBIMainRepository _userBIMainRepository;


        public WalletService()
        {
            _userBTCRepository = new UserBTCRepository();
            _userBIMainRepository = new UserBIMainRepository();
            //_historyBTCWithDrawRepository = new HistoryBTCWithDrawRepository();
            //_historyBIConvertRepository = new HistoryBIConvertRepository();
        }

        public double CoinWalletBalance(string username)
        {
            return _userBTCRepository.GetBTCWalletByUsername(username).Amount;
        }

        public UserBIMain GetUserBIMain(string username)
        {
            return _userBIMainRepository.GetTable().SingleOrDefault(x => x.Username == username);
        }
    }
}
