using FluentValidation;
using ICOCore.Entities.Extra;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System.Collections.Generic;

namespace ICOCore.Services.Base
{
    public class BaseService
    {
        protected InvestmentDataContext _dataContext;

        public BaseService()
        {
            _dataContext = new InvestmentDataContext();
        }

        protected List<PropertyError> Validate<T>(T model, AbstractValidator<T> validator)
        {
            var result = validator.Validate(model);
            var errors = new List<PropertyError>();
            if (!result.IsValid)
            {
                var failures = result.Errors;
                foreach (var f in failures)
                {
                    errors.Add(new PropertyError { PropertyName = f.PropertyName, ErrorMessage = f.ErrorMessage });
                }
            }
            return errors;
        }
    }
}
