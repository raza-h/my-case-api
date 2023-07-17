using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class LeadService : ILeadService
    {
        private readonly ApiDbContext dbContext;
        public LeadService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddLeadAsync(Lead lead)
        {
            try
            {
                await dbContext.Lead.AddAsync(lead);
                await dbContext.SaveChangesAsync();
                return lead.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Lead> GetLeadByIdAsync(int Id)
        {
            try
            {
                Lead lead = await dbContext.Lead.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return lead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Lead>> GetLeadsAsync()
        {
            try
            {
                List<Lead> lead = await dbContext.Lead.ToListAsync();
                return lead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Lead> UpdateLeadAsync(Lead lead)
        {
            try
            {
                dbContext.Entry(lead).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return lead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteLeadAsync(int Id)
        {
            try
            {
                Lead lead = await dbContext.Lead.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (lead != null)
                {
                    dbContext.Entry(lead).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
