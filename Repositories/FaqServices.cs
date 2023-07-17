using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class FaqServices : IFaqServices
    {
        private readonly ApiDbContext dbContext;
        public FaqServices(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region Note Tag
        public async Task<int> AddFaqAsync(Faq model)
        {
            try
            {
                await dbContext.Faq.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Faq> GetFaqByIdAsync(int Id)
        {
            try
            {
                Faq _afaq = await dbContext.Faq.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return _afaq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Faq>> GetFaqAsync()
        {
            try
            {
                List<Faq> _resultModel = await dbContext.Faq.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Faq> UpdateFaqAsync(Faq model)
        {
            try
            {
                dbContext.Entry(model).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteFaqAsync(int Id)
        {
            try
            {
                Faq faq = await dbContext.Faq.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(faq).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
