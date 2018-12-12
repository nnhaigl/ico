using FluentValidation;
using ICOCore.Dtos.Components;
using ICOCore.Utils.Common;

namespace ICOCore.Validator
{
    public class CfgProvideHelpPolicyValidator : AbstractValidator<CfgProvideHelpPolicyDto>
    {
        public CfgProvideHelpPolicyValidator()
        {
            RuleFor(x => x.Name).Length(1, 150).WithMessage("Name is from 0 to 500 characters.").NotEmpty().WithMessage("Name can not be empty.");
            RuleFor(x => x.Description).Length(0, 500).WithMessage("Description is from 0 to 500 characters.");
            RuleFor(x => x.ProfitRateTemp).Must(DoubleNumber).WithMessage("Profit Rate is not valid.");
            RuleFor(x => x.NumberOfDaysTemp).Must(IntegerNumber).WithMessage("Days is not valid.");
        }

        private bool DoubleNumber(CfgProvideHelpPolicyDto model, string input)
        {
            return CommonUtils.IsDouble(model.ProfitRateTemp);
        }

        private bool IntegerNumber(CfgProvideHelpPolicyDto model, string input)
        {
            return CommonUtils.IsInt(model.NumberOfDaysTemp);
        }

    }
}
