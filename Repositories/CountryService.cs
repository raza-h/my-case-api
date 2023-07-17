using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class CountryService : ICountryService
    {
        private readonly ApiDbContext dbContext;

        public CountryService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Country>> GetCountriesAsync()
        {
            try
            {
                List<Country> countries = await dbContext.Country.ToListAsync();
                return countries;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<States>> GetStatesAsync()
        {
            try
            {
                List<States> states = await dbContext.States.ToListAsync();
                return states;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
