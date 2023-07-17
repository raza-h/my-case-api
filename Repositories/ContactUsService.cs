using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApiDbContext dbContext;
        public ContactUsService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region ContactUs
        public async Task<int> AddContactAsync(ContactUs model)
        {
            try
            {
                await dbContext.ContactUs.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ContactUs> GetContactByIdAsync(int Id)
        {
            try
            {
                ContactUs contactUs = await dbContext.ContactUs.Where(x => x.ID == Id).FirstOrDefaultAsync();
                return contactUs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContactUs>> GetContactAsync()
        {
            try
            {
                List<ContactUs> _resultModel = await dbContext.ContactUs.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ContactUs> UpdateContactAsync(ContactUs model)
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
        public async Task DeleteContactAsync(int Id)
        {
            try
            {
                ContactUs contactUs = await dbContext.ContactUs.Where(x => x.ID == Id).FirstOrDefaultAsync();
                dbContext.Entry(contactUs).State = EntityState.Deleted;
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
