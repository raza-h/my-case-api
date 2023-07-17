using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class EventsServices : IEventsService
    {
        private readonly ApiDbContext dbContext;
        public EventsServices(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddEvents(Events model)
        {
            try
            {
                if (model.EventId == 0)
                {

                    await dbContext.Events.AddAsync(model);
                    await dbContext.SaveChangesAsync();
                    return model.EventId;
                }
                else
                {
                    Events _entity = await dbContext.Events.Where(x => x.EventId == model.EventId).FirstOrDefaultAsync();
                    _entity.Title = model.Title;
                    _entity.Description = model.Description;
                    _entity.ThemeColor = model.ThemeColor;
                    if (model.Start != null)
                    {
                        _entity.Start = model.Start;
                    }
                    dbContext.Entry(_entity).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return model.EventId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Events>> GetEvents(string userId)
        {
            try
            {
                List<Events> Events = await dbContext.Events.Where(x => x.UserId == userId && x.WorkflowId == null).ToListAsync();
                return Events;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Events>> GetWorkflowEvents(string userId,int id)
        {
            try
            {
                List<Events> Events = await dbContext.Events.Where(x => x.UserId == userId && x.WorkflowId != null && x.WorkflowId==id).ToListAsync();
                return Events;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Events> GetEventsByid(int Id)
        {
            try
            {
                Events _entity = await dbContext.Events.Where(x => x.EventId == Id).FirstOrDefaultAsync();
                return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Events> UpdateEvents(Events model)
        {
            try
            {
                Events _entity = await dbContext.Events.Where(x => x.EventId == model.EventId).FirstOrDefaultAsync();


                dbContext.Entry(_entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteEvents(int Id)
        {
            try
            {
                Events model = await dbContext.Events.Where(x => x.EventId == Id).FirstOrDefaultAsync();
                dbContext.Entry(model).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
