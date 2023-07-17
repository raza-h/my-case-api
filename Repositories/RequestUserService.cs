using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class RequestUserService : IRequestUserService
    {
        private readonly ApiDbContext dbContext;
        public RequestUserService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddRequestUser(RequestUsers requestUsers)
        {
            try
            {
                await dbContext.RequestUsers.AddAsync(requestUsers);
                await dbContext.SaveChangesAsync();
                return requestUsers.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RequestUsers>> GetRequestUser(string Status = "")
        {
            try
            {

                List<RequestUsers> users = new List<RequestUsers>();
                if (Status == "")
                    users = await dbContext.RequestUsers.ToListAsync();
                else if (Status == "Approved")
                    users = await dbContext.RequestUsers.Where(x => x.VerificationStatus == VerificationStatus.Approved).ToListAsync();
                else if (Status == "Pending")
                    users = await dbContext.RequestUsers.Where(x => x.VerificationStatus == VerificationStatus.Pending).ToListAsync();
                else if (Status == "Rejected")
                    users = await dbContext.RequestUsers.Where(x => x.VerificationStatus == VerificationStatus.Rejected).ToListAsync();

                return users;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RequestUsers> GetByEmailAsync(string Email)
        {
            try
            {
                RequestUsers user = await dbContext.RequestUsers.Where(x => x.Email == Email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RequestUsers> GetRequestUserById(string Id)
        {
            try
            {
                RequestUsers userModel = await dbContext.RequestUsers.Where(x=>x.Id==Convert.ToInt32(Id)).FirstOrDefaultAsync();
                return userModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ChangeStatus(string Id ,string Status)
        {
            try
            {
                var user = dbContext.RequestUsers.First(x => x.Id == Convert.ToInt32(Id));
                if (Status == "Approved")
                {
                    user.VerificationStatus = VerificationStatus.Approved;
                    dbContext.Entry(user).CurrentValues.SetValues(user);
                    await dbContext.SaveChangesAsync();
                }


                else if (Status == "Pending")
                {
                    user.VerificationStatus = VerificationStatus.Pending;
                    dbContext.Entry(user).CurrentValues.SetValues(user);
                    await dbContext.SaveChangesAsync();
                }
                else if (Status == "Rejected")
                {
                    user.VerificationStatus = VerificationStatus.Rejected;
                    dbContext.Entry(user).CurrentValues.SetValues(user);
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
