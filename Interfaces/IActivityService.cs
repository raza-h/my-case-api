using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
   public  interface IActivityService
    {
        Task<List<AdminActivity>> GetAdminActivities(string UserId, bool IsFilterByDates, DateTime? Date1 = null, DateTime? Date2 = null);
        Task AddActivity(string operationName, string actionDetail, string userId);
    }
}
