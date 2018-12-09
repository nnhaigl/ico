using ICOCore.Queries.Base;
using ICOCore.Repositories;

namespace ICOCore.Queries.Components
{
    public class BTCStoreTransactionQuery : BaseQuery<BTCStoreTransaction>
    {
        public BTCStoreTransactionQuery()
        {
            Type = -1; // Default không tìm kiếm trường type
        }

        public int Type { set; get; }
        public string Username { set; get; }
        public string Date { set; get; }

    }
}
