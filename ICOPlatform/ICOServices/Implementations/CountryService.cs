using ICOCore.Messages.Base;
using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICORepository.Implementations.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class CountryService : BaseService
    {

        private CountryRepository _countryRepository;

        public CountryService()
        {
            _countryRepository = new CountryRepository();
        }

        public Country GetByCode(string countryCode)
        {
            return _countryRepository.GetTable().SingleOrDefault(x => x.Code == countryCode);
        }

        public BaseListResponse<Country> BuildDropDown()
        {
            var response = new BaseListResponse<Country>();

            try
            {
                var countires = new List<Country>();
                countires.Add(new Country { Code = "", Name = "-- Select your country --" });
                var allCountries = _countryRepository.GetAll().OrderBy(x => x.Code).ToList();

                foreach (var co in allCountries)
                {
                    co.Name = string.Concat("( + ", co.PhoneCode, ") ", co.Name);
                }

                countires.AddRange(allCountries);

                response.Data = countires;
                response.TotalItems = response.Data != null ? response.Data.Count : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

    }
}
