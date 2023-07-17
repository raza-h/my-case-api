using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ICountryService
    {
        public Task<List<Country>> GetCountriesAsync();
        public Task<List<States>> GetStatesAsync();
    }
}
