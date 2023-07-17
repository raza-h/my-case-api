using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ApiDbContext dbContext;
        public WorkflowService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddWorkflowAsync(WorkflowBase data)
        {
            try
            {
                await dbContext.WorkflowBase.AddAsync(data);
                await dbContext.SaveChangesAsync();
                return data.WorkflowId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AttachWorkflowToCaseAsync(WorkflowAttach data)
        {
            try
            {
                var getdata = dbContext.WorkflowAttach.Where(x => x.WorkflowId == data.WorkflowId && x.CaseId == data.CaseId).FirstOrDefault();
                if (getdata != null)
                {
                    return 0;
                }
                else
                {
                    await dbContext.WorkflowAttach.AddAsync(data);
                    await dbContext.SaveChangesAsync();
                    return data.Id;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<WorkflowBase> GetWorkflowByIdAsync(int Id)
        {
            try
            {
                WorkflowBase data = await dbContext.WorkflowBase.Where(x => x.WorkflowId == Id).FirstOrDefaultAsync();
                if (data != null)
                {
                    List<Decuments> documentdata = await dbContext.Decuments.Where(x => x.WorkflowId == data.WorkflowId).ToListAsync();
                    if (documentdata != null)
                        data.documents = documentdata;
                    List<Tasks> taskdata = await dbContext.Tasks.Where(x => x.WorkflowId == data.WorkflowId).ToListAsync();
                    if (taskdata != null)
                        data.tasks = taskdata;
                    List<Events> eventdata = await dbContext.Events.Where(x => x.WorkflowId == data.WorkflowId).ToListAsync();
                    if (eventdata != null)
                        data.events = eventdata;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<WorkflowBase>> GetWorkflowAsync()
        {
            try
            {
                List<WorkflowBase> data = new List<WorkflowBase>();
                data = await dbContext.WorkflowBase.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<WorkflowBase> UpdateWorkflowAsync(WorkflowBase task)
        {
            try
            {
                var gettask = dbContext.Tasks.ToList().Where(x => x.Id == task.WorkflowId).FirstOrDefault();

                //if (task.CaseName != null)
                //{
                //    gettask.CaseName = task.CaseName;
                //}
                //if (task.ClientId != null && task.ClientId != "")
                //{
                //    gettask.ClientId = task.ClientId;
                //    gettask.StaffId = null;
                //}
                //if (task.Description != null)
                //{
                //    gettask.Description = task.Description;
                //}
                //if (task.Name != null)
                //{
                //    gettask.Name = task.Name;
                //}
                //if (task.Priority != null)
                //{
                //    gettask.Priority = task.Priority;
                //}
                //if (task.StaffId != null && task.StaffId != "")
                //{
                //    gettask.StaffId = task.StaffId;
                //    gettask.ClientId = null;
                //}
                //if (task.Status != null)
                //{
                //    gettask.Status = task.Status;
                //}
                dbContext.Entry(gettask).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteWorkflowAsync(int Id)
        {
            try
            {
                Tasks task = await dbContext.Tasks.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    dbContext.Entry(task).State = EntityState.Deleted;
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
