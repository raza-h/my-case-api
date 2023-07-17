using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ITimeLineService
    {
        Task<int> AddTimeLineAsync(TimeLine timeLine);
        Task<List<TimeLine>> GetTimeLinesAsync(string userId, string userType);
        Task<List<TimeLine>> GetTimeLinesReminder();
        Task<List<TimeLine>> GetTimeLineByID(int id);
        Task<TimeLine> updateTimeLine(TimeLine timeLine);
        Task DeleteTimeLineAsync(int ID);
        Task UpdateTimeLineAsync(TimeLine timeLine);
    }
}
