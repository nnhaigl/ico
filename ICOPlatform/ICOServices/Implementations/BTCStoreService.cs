using ICOCore.Infrastructures.Enums;
using ICOCore.Messages.Base;
using ICOCore.Messages.Requests;
using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICOCore.Utils.Common;
using ICOCore.Utils.Encrypt;
using ICORepository.Implementations.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class BTCStoreService : BaseService
    {

        private BTCStoreRepository _btcStoreRepository;
        private BTCStoreTransactionRepository _btcStoreTransactionRepository;

        public BTCStoreService()
        {
            _btcStoreRepository = new BTCStoreRepository();
            _btcStoreTransactionRepository = new BTCStoreTransactionRepository();
        }

        /// <summary>
        /// Lấy random địa chỉ ví nhận tiền của hệ thống
        /// </summary>
        /// <returns></returns>
        public BaseSingleResponse<string> GetRandomAddressForReceive()
        {
            var response = new BaseSingleResponse<string>();
            try
            {
                response.Value = _btcStoreRepository.GetRandomAddressForReceive();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        /// <summary>
        /// User nạp tiền vào hệ thống
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse DepositBTC(DepositBTCRequest request)
        {
            var response = new BaseResponse();

            try
            {
                _dataContext.Connection.Open();
                System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = transaction;

                try
                {
                    // 1. Lấy địa chỉ ví của hệ thống để nhận tiền từ user 
                    //BTCStore store = _btcStoreRepository.GetAddressForReceive();
                    string addressForReceive = request.AddressForReceive; //store.Address;

                    // 2.Lấy User BTC
                    UserBTC userBTC = _dataContext.UserBTCs.Single(x => x.Username == request.Username);

                    // 3. Lấy UserInfo
                    UserInfo userInfo = _dataContext.UserInfos.Single(x => x.Username == request.Username);

                    // check valid 
                    BTCStore btcStoreForReceiveInfo = _btcStoreRepository.GetAddressForReceive(addressForReceive);
                    if (btcStoreForReceiveInfo == null || userBTC == null
                        || userInfo == null || request.Amount <= 0)
                    {
                        throw new ArgumentNullException("Invalid request parameter");
                    }

                    // 4 tạo transaction
                    BTCStoreTransaction storeTransaction = new BTCStoreTransaction();
                    storeTransaction.Type = (int)BTCStoreTransactionTypeEnum.DEPOSIT;
                    storeTransaction.StoreBitAddress = addressForReceive;
                    storeTransaction.UserBitAddress = userBTC.WalletID;
                    storeTransaction.TransactionHash = SaltHelper.GetUniqueKey();
                    storeTransaction.Amount = request.Amount;
                    storeTransaction.Amount = CommonUtils.FoatBTCAmount(storeTransaction.Amount);

                    storeTransaction.UserId = userInfo.Id;
                    storeTransaction.Username = userInfo.Username;
                    storeTransaction.Fee = 0;
                    storeTransaction.CreatedDate = DateTime.Now;
                    storeTransaction.ModifiedDate = storeTransaction.CreatedDate;
                    storeTransaction.Status = (int)BTCStoreTransactionStatusEnum.NOT_CONFIRMED;
                    storeTransaction.Rate = 0;
                    storeTransaction.GUID = System.Guid.NewGuid();
                    storeTransaction.Note = null;

                    _dataContext.BTCStoreTransactions.InsertOnSubmit(storeTransaction);

                    _dataContext.SubmitChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                }
                finally
                {
                    try
                    {
                        if (null != _dataContext.Connection)
                            _dataContext.Connection.Close();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseListResponse<BTCStoreTransaction> SearchBTCTransaction(BTCStoreTransactionQuery query)
        {
            var response = new BaseListResponse<BTCStoreTransaction>();

            try
            {
                int count = 0;
                response.Data = _btcStoreTransactionRepository.Search(query, out count);
                response.TotalItems = count;
                response.PageIndex = query.PageIndex;
                response.PageSize = query.PageSize;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        /// <summary>
        /// Confirm đã nhận được Deposit từ User
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public BaseResponse ConfirmBTCStoreTransactionForReceive(string guid)
        {
            return SetBTCStoreTransactionForReceive(guid, BTCStoreTransactionStatusEnum.CONFIRMED);
        }

        /// <summary>
        /// Reject Deposit từ User
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public BaseResponse RejectBTCStoreTransactionForReceive(string guid)
        {
            return SetBTCStoreTransactionForReceive(guid, BTCStoreTransactionStatusEnum.REJECTED);
        }

        /// <summary>
        /// Set trạng thái cho loại giao dịch là Receive (User Deposit)
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public BaseResponse SetBTCStoreTransactionForReceive(string guid, BTCStoreTransactionStatusEnum status)
        {
            var response = new BaseResponse();

            try
            {
                var transGUID = new Guid(guid);

                _dataContext.Connection.Open();
                System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = transaction;

                try
                {

                    BTCStoreTransaction btcTransaction = _dataContext.BTCStoreTransactions.Single(x => x.GUID == transGUID);
                    BTCStore btcStore = _dataContext.BTCStores.Single(x => x.Address == btcTransaction.StoreBitAddress);

                    if (btcTransaction.Type == (int)BTCStoreTransactionTypeEnum.DEPOSIT)
                    {
                        if (BTCStoreTransactionStatusEnum.CONFIRMED == status)
                        {
                            if (btcTransaction.Status == (int)BTCStoreTransactionStatusEnum.NOT_CONFIRMED)
                            {
                                btcTransaction.Status = (int)BTCStoreTransactionStatusEnum.CONFIRMED;
                                btcStore.Amount = btcStore.Amount + btcTransaction.Amount;
                                btcStore.ModifiedDate = DateTime.Now;

                                // lấy ví UserBTC
                                UserBTC userBTC = _dataContext.UserBTCs.Single(x => x.Username == btcTransaction.Username);
                                userBTC.Amount += btcTransaction.Amount;
                                userBTC.Amount = CommonUtils.FoatBTCAmount(userBTC.Amount);
                            }
                        }
                        else if (BTCStoreTransactionStatusEnum.REJECTED == status)
                        {
                            if (btcTransaction.Status == (int)BTCStoreTransactionStatusEnum.NOT_CONFIRMED)
                            {
                                btcTransaction.Status = (int)BTCStoreTransactionStatusEnum.REJECTED;
                                btcTransaction.ModifiedDate = DateTime.Now;
                            }
                        }
                    }

                    _dataContext.SubmitChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                }
                finally
                {
                    try
                    {
                        if (null != _dataContext.Connection)
                            _dataContext.Connection.Close();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }


    }
}
