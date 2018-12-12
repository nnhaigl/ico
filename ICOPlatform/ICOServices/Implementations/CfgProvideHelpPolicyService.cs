using ICOCore.Dtos.Components;
using ICOCore.Infrastructures.Enums;
using ICOCore.Messages.Base;
using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICOCore.Utils;
using ICOCore.Validator;
using ICORepository.Implementations.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class CfgProvideHelpPolicyService : BaseService
    {

        private CfgProvideHelpPolicyRepository _repository;
        public CfgProvideHelpPolicyService()
        {
            _repository = new CfgProvideHelpPolicyRepository();
        }

        public CfgProvideHelpPolicy SingleById(long id)
        {
            return _repository.SingleEnableById(id);
        }

        public BaseSingleResponse<CfgProvideHelpPolicyDto> GetById(long id)
        {
            var response = new BaseSingleResponse<CfgProvideHelpPolicyDto>();
            try
            {
                CfgProvideHelpPolicy policy = SingleById(id);
                if (policy != null)
                {
                    CfgProvideHelpPolicyDto dto = MapperUtils.ConvertTo<CfgProvideHelpPolicy, CfgProvideHelpPolicyDto>(policy);
                    dto.NumberOfDaysTemp = policy.NumberOfDays.ToString();
                    dto.ProfitRateTemp = policy.ProfitRate.ToString();

                    response.Value = dto;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseSingleResponse<CfgProvideHelpPolicyDto> Add(CfgProvideHelpPolicyDto dto)
        {
            var response = new BaseSingleResponse<CfgProvideHelpPolicyDto>();

            try
            {
                var errors = Validate<CfgProvideHelpPolicyDto>(dto, new CfgProvideHelpPolicyValidator());
                if (errors.Count() > 0)
                    return new BaseSingleResponse<CfgProvideHelpPolicyDto>(dto, errors);

                var policy = new CfgProvideHelpPolicy();

                policy.Name = dto.Name;
                policy.Description = dto.Description;

                policy.ProfitRate = double.Parse(dto.ProfitRateTemp);
                policy.NumberOfDays = int.Parse(dto.NumberOfDaysTemp);
                policy.CreatedDate = DateTime.Now;
                policy.ModifiedDate = policy.CreatedDate;
                policy.Status = (int)CfgProvideHelpPolicyStatusEnum.ENABLE;

                _repository.Add(policy);
                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseSingleResponse<CfgProvideHelpPolicyDto> Update(CfgProvideHelpPolicyDto dto)
        {
            var response = new BaseSingleResponse<CfgProvideHelpPolicyDto>();

            try
            {
                var errors = Validate<CfgProvideHelpPolicyDto>(dto, new CfgProvideHelpPolicyValidator());
                if (errors.Count() > 0)
                    return new BaseSingleResponse<CfgProvideHelpPolicyDto>(dto, errors);

                var policy = _repository.Get(dto.Id);

                policy.Name = dto.Name;
                policy.Description = dto.Description;
                policy.ProfitRate = double.Parse(dto.ProfitRateTemp);

                policy.ProfitRate = double.Parse(dto.ProfitRateTemp);
                policy.NumberOfDays = int.Parse(dto.NumberOfDaysTemp);
                policy.ModifiedDate = DateTime.Now;

                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseResponse Delete(long id)
        {
            var response = new BaseResponse();

            try
            {
                _repository.Delete(id);
                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }


        public BaseListResponse<CfgProvideHelpPolicy> GetAll()
        {
            var response = new BaseListResponse<CfgProvideHelpPolicy>();
            try
            {
                response.Data = _repository.GetAll().OrderBy(x => x.NumberOfDays).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseListResponse<CfgProvideHelpPolicy> GetAllEnablePackageDropdown()
        {
            var response = new BaseListResponse<CfgProvideHelpPolicy>();
            try
            {
                var dropdownPolicies = new List<CfgProvideHelpPolicy>();
                dropdownPolicies.Add(new CfgProvideHelpPolicy { Id = 0, Display = "-- Select your package --" });

                var allPolicies = _repository.GetAllEnablePackage();
                foreach (var po in allPolicies)
                {
                    po.Display = string.Concat(po.Name, " (", po.NumberOfDays, " Days ) - Profit Rate : ", po.ProfitRate, " (", po.ProfitRate * 100, " % / Day)");
                }
                dropdownPolicies.AddRange(allPolicies);
                response.Data = dropdownPolicies;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }



    }
}
