using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class CompanyService : ICompanyService
    {
        private readonly ApiDbContext dbContext;

        public CompanyService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddCompanyAsync(Company company)
        {
            try
            {
                await dbContext.Company.AddAsync(company);
                await dbContext.SaveChangesAsync();
                return company.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(int Id)
        {
            try
            {
                Company company = await dbContext.Company.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(company).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Company>> GetAsync()
        {
            try
            {
                List<Country> countries = await dbContext.Country.ToListAsync();
                List<Company> companies = await dbContext.Company.ToListAsync();

            
                if (companies != null && companies.Count > 0 && countries != null && countries.Count > 0)
                {
                    foreach(var company in companies)
                    {
                        company.CountryName = countries.Where(x => x.Id == company.CountryId).Select(c => c.Name).FirstOrDefault();
                    }
                }
                return companies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Company> GetByIdAsync(int Id)
        {
            try
            {
                Company company = await dbContext.Company.Where(x => x.Id == Id).FirstOrDefaultAsync();
                var customFields = await dbContext.CustomField.Where(x => x.Type == "Companies").ToListAsync();
                if (customFields != null)
                {
                    company.customField = customFields;
                }
                var customFieldsValue = await dbContext.CFieldValue.Where(x => x.ConcernID == company.Id).ToListAsync();
                if (customFieldsValue != null)
                {
                    company.cfieldValue = customFieldsValue;
                }
                return company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            try
            {
                dbContext.Entry(company).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
