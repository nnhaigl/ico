using ICOCore.Entities.Extra;
using ICOCore.Infrastructures.Constants;
using ICOCore.Infrastructures.Enums;
using ICOCore.Messages.Base;
using ICOCore.Queries.Components;
using ICOCore.Services.Base;
using ICOCore.Utils.Common;
using ICOCore.Utils.Types;
using ICOCore.ViewModels;
using ICORepository.Implementations.Histories;
using ICORepository.Implementations.Stores;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICOServices.Implementations
{
    public class ReportService : BaseService
    {

        public BaseResponse GetGeneralReportToday()
        {
            var query = new GeneralReportQuery
            {
                FromDate = DateTime.Now.ToString(CommonConstants.DATE_FORMAT),
                ToDate = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
            };
            return this.GetGeneralReportByDay(query);
        }

        public BaseResponse GetGeneralReportByDay(GeneralReportQuery query)
        {
            var response = new BaseListResponse<GeneralReportByDayVM>();
            var errors = new List<PropertyError>();

            GeneralReportByDayVM temp = null;
            try
            {
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;

                if (string.IsNullOrWhiteSpace(query.FromDate) && string.IsNullOrWhiteSpace(query.ToDate))
                {
                    // first request
                    fromDate = toDate.AddDays(-15);
                }
                else
                {
                    // chỉ giới hạn hiển bị báo cáo tổng hợp trong khoảng 5 ngày liên tiếp để tránh hít DB quá nhiều
                    if (!string.IsNullOrWhiteSpace(query.FromDate))
                    {
                        bool isDate = CommonUtils.ToDate(query.FromDate, out fromDate);
                        if (!isDate)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => query.FromDate),
                                ErrorMessage = "Invalid From Date.Must be in format dd/mm/yyyy"
                            });
                            return new BaseSingleResponse<bool>(false, errors);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(query.ToDate))
                    {
                        bool isDate = CommonUtils.ToDate(query.ToDate, out toDate);
                        if (!isDate)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => query.ToDate),
                                ErrorMessage = "Invalid To Date.Must be in format dd/mm/yyyy"
                            });
                            return new BaseSingleResponse<bool>(false, errors);
                        }
                    }

                    if ((toDate.Date - fromDate.Date).TotalDays < 0)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => query.ToDate),
                            ErrorMessage = "To Date must be greater than From Date."
                        });
                        return new BaseSingleResponse<bool>(false, errors);
                    }

                    if ((toDate.Date - fromDate.Date).TotalDays > 15)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => query.ToDate),
                            ErrorMessage = "The range between dates is 15 days."
                        });
                        return new BaseSingleResponse<bool>(false, errors);
                    }
                }

                var reports = new List<GeneralReportByDayVM>();

                _dataContext.Connection.Open();
                System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = transaction;

                try
                {
                    while ((toDate.Date - fromDate.Date).TotalDays >= 0)
                    {

                        temp = new GeneralReportByDayVM();

                        temp.Date = fromDate;

                        // 1. TotalDepositBTC tổng số BTC đã nạp
                        var btcData = _dataContext.BTCStoreTransactions.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                                          && x.CreatedDate.Month == fromDate.Month
                                                                                          && x.CreatedDate.Day == fromDate.Day
                                                                                          && x.Type == (int)BTCStoreTransactionTypeEnum.DEPOSIT)
                                                                                          .Select(x => x.Amount)
                                                                                          .ToList();
                        if (btcData != null) temp.TotalDepositBTC = btcData.Sum(x => x);

                        // 2.TotalAmountPH Tổng số BTC PH
                        var phData = _dataContext.ProvideHelps.Where(x => x.CreateDate.Year == fromDate.Year
                                                                                   && x.CreateDate.Month == fromDate.Month
                                                                                   && x.CreateDate.Day == fromDate.Day)
                                                                                   .Select(x => x.BTCAmount)
                                                                                   .ToList();
                        if (phData != null) temp.TotalAmountPH = phData.Sum(x => x);

                        // 3. TotalNewMember Tổng số thành viên mới
                        temp.TotalNewMember = _dataContext.Accounts.Count(x => x.CreatedDate.Year == fromDate.Year
                                                                          && x.CreatedDate.Month == fromDate.Month
                                                                          && x.CreatedDate.Day == fromDate.Day);

                        // 4.TotalDirectCom Tổng amount hoa hồng trực tiếp
                        var directComData = _dataContext.LogPHComissions.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                       && x.CreatedDate.Month == fromDate.Month
                                                                       && x.CreatedDate.Day == fromDate.Day)
                                                                       .Select(x => x.BIAmount)
                                                                       .ToList();

                        if (directComData != null) temp.TotalDirectCom = directComData.Sum(x => x);

                        // 5.TotalMatchingCom Tổng hoa hồng cân cặp
                        var matchingComData = _dataContext.LogMatchingCommissions.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                                  && x.CreatedDate.Month == fromDate.Month
                                                                                  && x.CreatedDate.Day == fromDate.Day)
                                                                                  .Select(x => x.CommissionAmount)
                                                                                  .ToList();
                        if (matchingComData != null) temp.TotalMatchingCom = matchingComData.Sum(x => x);


                        // 6.TotalLeaderCom Tổng hoa hồng Leader
                        temp.TotalLeaderCom = 0;

                        // 7.TotalUsedToken : Tổng số lượng mà user dùng cho PH/GH
                        var usedTokenData = _dataContext.TransactionTokens.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                               && x.CreatedDate.Month == fromDate.Month
                                                                               && x.CreatedDate.Day == fromDate.Day
                                                                               && (x.TransferType == (int)TransactionTokenTransferTypeEnum.PAID_FOR_GET_HELP
                                                                                        || x.TransferType == (int)TransactionTokenTransferTypeEnum.PAID_FOR_PROVIDE_HELP))
                                                                               .Select(x => x.NumberTransToken)
                                                                               .ToList();
                        if (usedTokenData != null) temp.TotalUsedToken = usedTokenData.Sum(x => x);

                        // 8.TotalBuyToken Tổng số lượng Token mà user đã mua
                        var buyTokenData = _dataContext.TransactionTokens.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                           && x.CreatedDate.Month == fromDate.Month
                                                                           && x.CreatedDate.Day == fromDate.Day
                                                                           && x.TransferType == (int)TransactionTokenTransferTypeEnum.BUY_FROM_SYSTEM)
                                                                           .Select(x => x.NumberTransToken)
                                                                           .ToList();
                        if (buyTokenData != null) temp.TotalBuyToken = buyTokenData.Sum(x => x);

                        // 9.TotalTokenLeftOfMember Tổng số lượng Token còn lại trong tài khoản của user

                        // 10 .TotalWithdrawBTC Tổng rút BTC

                        var withDrawData = _dataContext.HistoryBTCWithDraws.Where(x => x.CreatedDate.Year == fromDate.Year
                                                                      && x.CreatedDate.Month == fromDate.Month
                                                                      && x.CreatedDate.Day == fromDate.Day)
                                                                      .Select(x => x.Amount)
                                                                      .ToList();

                        if (withDrawData != null) temp.TotalWithdrawBTC = withDrawData.Sum(x => x);

                        // 11. Total GH: Tổng amount GetHelp
                        var getHelpData = _dataContext.GetHelps.Where(x => x.GetDate.Year == fromDate.Year
                                                                   && x.GetDate.Month == fromDate.Month
                                                                   && x.GetDate.Day == fromDate.Day)
                                                                   .Select(x => x.BIAmountNeedToken + x.BIAmountNotNeedToken)
                                                                   .ToList();

                        if (getHelpData != null) temp.TotalAmountGH = getHelpData.Sum(x => x);

                        temp.TotalDepositBTC = CommonUtils.FoatBTCAmount(temp.TotalDepositBTC);
                        temp.TotalAmountPH = CommonUtils.FoatBTCAmount(temp.TotalAmountPH);
                        temp.TotalDirectCom = CommonUtils.FoatBTCAmount(temp.TotalDirectCom);
                        temp.TotalMatchingCom = CommonUtils.FoatBTCAmount(temp.TotalMatchingCom);
                        temp.TotalLeaderCom = CommonUtils.FoatBTCAmount(temp.TotalLeaderCom);
                        temp.TotalAmountGH = CommonUtils.FoatBTCAmount(temp.TotalAmountGH);

                        reports.Add(temp);

                        fromDate = fromDate.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.Message + ex.StackTrace;
                }
                finally
                {
                    transaction.Rollback();
                }

                reports = reports.OrderByDescending(x => x.Date).ToList();

                response.Data = reports;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                //response.Message = ex.Message + ex.StackTrace;
            }
            return response;
        }

        public BaseResponse TotalReport()
        {
            var response = new BaseSingleResponse<TotalReportVM>();

            try
            {
                var vm = new TotalReportVM();
                BTCStoreTransactionRepository btcRepo = new BTCStoreTransactionRepository();
                HistoryBTCWithDrawRepository withdrawRepo = new HistoryBTCWithDrawRepository();

                vm.TotalDeposit = btcRepo.GetTable().Where(x => x.Type == (int)BTCStoreTransactionTypeEnum.DEPOSIT &&
                                                                x.Status == (int)BTCStoreTransactionStatusEnum.CONFIRMED)
                                                                .Sum(x => x.Amount);
                vm.TotalWithDraw = withdrawRepo.GetTable().Where(x => x.Status == (int)HistoryBTCWithDrawStatusEnum.CONFIRMED_WITHDRAW_OK
                                                                    || x.Status == (int)HistoryBTCWithDrawStatusEnum.CONFIRMED_AND_WAITING_BLOCKCHAIN)
                                                                    .Sum(x => x.Amount);
                vm.TotalDeposit = CommonUtils.FoatBTCAmount(vm.TotalDeposit);
                vm.TotalWithDraw = CommonUtils.FoatBTCAmount(vm.TotalWithDraw);

                response.Value = vm;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

    }
}
