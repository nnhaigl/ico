using ICOCore.Infrastructures.Enums;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICORepository.Implementations.Stores
{
    public class BTCStoreRepository : Repository<BTCStore>
    {
        public BTCStore GetFirstAddressForReceive()
        {
            return GetTable().Single(x => x.Type == (int)BTCStoreTypeEnum.FOR_RECEIVE && x.Status == 0);
        }

        public BTCStore GetAddressForReceive(string address)
        {
            return GetTable().Single(x => x.Type == (int)BTCStoreTypeEnum.FOR_RECEIVE && x.Status == 0 && x.Address == address);
        }

        public string GetRandomAddressForReceive()
        {
            List<BTCStore> stores = GetAll();
            string address = string.Empty;

            if (stores != null && stores.Any())
            {
                Random r = new Random();
                int num = r.Next(stores.Count);
                var randomFromList = stores[num];
                address = randomFromList.Address;
            }

            return address;
        }

    }
}
