using ICOCore.Messages.Base;
using ICOCore.Messages.Requests;
using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOServices.Implementations;
using ICOWebCore.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICOWeb.API
{
    [RoutePrefix("bizzApi/btcstore")]
    public class BTCStoreApiController : BaseFrontAPIController
    {
        private BTCStoreService _service;

        public BTCStoreApiController()
        {
            _service = new BTCStoreService();
        }

        [HttpPost]
        [Route("Deposit")]
        public BaseResponse Deposit(DepositBTCRequest request)
        {
            request.Username = UserHeader();
            return _service.DepositBTC(request);
        }

        [HttpGet]
        [Route("ReceiveAddress")]
        public BaseSingleResponse<string> GetRandomAddressForReceive()
        {
            return _service.GetRandomAddressForReceive();
        }

        [HttpPost]
        [Route("transaction")]
        public BaseListResponse<BTCStoreTransaction> SearchBTCTransaction(BTCStoreTransactionQuery query)
        {
            query.Username = UserHeader();
            return _service.SearchBTCTransaction(query);
        }

    }
}
