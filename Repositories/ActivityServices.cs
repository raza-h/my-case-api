using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace MyCaseApi.Repositories
{
    public class ActivityServices : IActivityService
    {
        private readonly ApiDbContext dbContext;
        public ActivityServices(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddActivity(string operationName, string actionDetail, string userId)
        {
            try
            {
                AdminActivity activity = new AdminActivity();
                activity.Date = DateTime.Now;
                activity.CreatedBy = userId;
                activity.OperationName = operationName;
                activity.ActionDetail = actionDetail;
                await dbContext.AdminActivity.AddAsync(activity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AdminActivity>> GetAdminActivities(string UserId, bool IsFilterByDates, DateTime? Date1 = null, DateTime? Date2 = null)
        {
            try
            {
                List<AdminActivity> activities = new List<AdminActivity>();
                var users = await dbContext.User.Where(x => x.ParentId == UserId || x.Id == UserId).ToListAsync();
                if (users != null && users.Count > 0)
                {
                    string[] UserIds = users.Select(x => x.Id).ToArray();
                    activities = await dbContext.AdminActivity.Where(x => UserIds.Contains(x.CreatedBy)).ToListAsync();
                }
                if (IsFilterByDates)
                    activities = activities.Where(x => x.Date > Date1 && x.Date < Date2).ToList();
                if (activities != null && activities.Count > 0)
                    activities = activities.OrderByDescending(x => x.Date).ToList();
                return activities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
