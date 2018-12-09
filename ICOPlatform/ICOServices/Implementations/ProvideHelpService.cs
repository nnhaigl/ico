using ICOCore.Dtos.Components;
using ICOCore.Messages.Base;
using ICOCore.Queries.Components;
using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICOCore.Utils;
using ICOCore.Utils.Common;
using ICOCore.Utils.Types;
using ICORepository.Implementations.Investment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class ProvideHelpService : BaseService
    {

        private ProvideHelpRepository _repository;

        public ProvideHelpService()
        {
            _repository = new ProvideHelpRepository();
        }

        public List<ProvideHelp> AllRunningPH(string username)
        {
            return _repository.AllRunningPH(username);
        }

        public BaseListResponse<ProvideHelpDto> Search(ProvideHelpQuery query)
        {
            var response = new BaseListResponse<ProvideHelpDto>();

            try
            {
                int count = 0;
                var provideHelps = _repository.Search(query, out count);
                response.TotalItems = count;
                response.PageIndex = query.PageIndex;
                response.PageSize = query.PageSize;

                ProvideHelp fake = new ProvideHelp();
                string[] excludeProperties =
                {
                     TypeHelper.GetPropertyName(() => fake.Id)
                };

                var provideHelpDtos = MapperUtils.ConvertToList<ProvideHelp, ProvideHelpDto>(provideHelps, excludeProperties);

                if (provideHelpDtos != null)
                {
                    foreach (var dto in provideHelpDtos)
                    {
                        // tinh so ngay sap complete (ngay lai~ sap do? vao BI Balance)
                        double daysStartTransferBI = (dto.StartDate - dto.CreateDate).TotalDays;
                        double daysPassed = (DateTime.Now - dto.CreateDate).TotalDays;
                        dto.DaysPassed = (int)daysPassed;

                        if (daysPassed >= daysStartTransferBI)
                            dto.PercentComplete = 100;
                        else
                            dto.PercentComplete = (int)((daysPassed / daysStartTransferBI) * 100);

                        // tinh so tien BI da kiem duoc
                        dto.EarningBIAmount = CommonUtils.FoatBTCAmount(dto.ProfitRate * dto.BTCAmount * daysPassed);
                    }
                }

                response.Data = provideHelpDtos;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }
        

    }
}
