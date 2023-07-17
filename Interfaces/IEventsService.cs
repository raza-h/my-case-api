using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
  public  interface IEventsService
    {
        Task<int> AddEvents(Events model);
        Task<List<Events>> GetEvents(string userId);
        Task<List<Events>> GetWorkflowEvents(string userId,int id);
        Task<Events> GetEventsByid(int EventId);
        Task<Events> UpdateEvents(Events model);
        Task DeleteEvents(int EventId);
    }
}
