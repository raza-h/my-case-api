using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class TaskService : ITaskService
    {
        private readonly ApiDbContext dbContext;
        public TaskService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddTaskAsync(Tasks task)
        {
            try
            {
                await dbContext.Tasks.AddAsync(task);
                await dbContext.SaveChangesAsync();
                return task.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddWorkflowTaskAsync(Tasks task)
        {
            try
            {
                await dbContext.Tasks.AddAsync(task);
                await dbContext.SaveChangesAsync();
                return task.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Tasks> GetTaskByIdAsync(int Id)
        {
            try
            {
                Tasks task = await dbContext.Tasks.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (task != null && task.CaseId != null && task.CaseId > 0)
                {
                    CaseDetail caseDetail = await dbContext.CaseDetail.Where(x => x.Id == task.CaseId).FirstOrDefaultAsync();
                    if (caseDetail != null)
                        task.CaseName = caseDetail.CaseName;
                }
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Tasks>> GetTasksAsync(string userId = "", string userType = "")
        {
            try
            {
                List<Tasks> tasks = new List<Tasks>();
                if (string.IsNullOrEmpty(userId))
                    tasks = await dbContext.Tasks.Where(x => x.WorkflowId == null).ToListAsync();
                else if (userType == "client")
                    tasks = await dbContext.Tasks.Where(x => x.ClientId == userId && x.WorkflowId == null).ToListAsync();
                else
                    tasks = await dbContext.Tasks.Where(x => x.StaffId == userId && x.WorkflowId == null).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Tasks> UpdateTaskAsync(Tasks task)
        {
            try
            {
                var gettask = dbContext.Tasks.ToList().Where(x => x.Id == task.Id).FirstOrDefault();

                if (task.CaseName != null)
                {
                    gettask.CaseName = task.CaseName;
                }
                if (task.ClientId != null && task.ClientId != "")
                {
                    gettask.ClientId = task.ClientId;
                    gettask.StaffId = null;
                }
                if (task.Description != null)
                {
                    gettask.Description = task.Description;
                }
                if (task.Name != null)
                {
                    gettask.Name = task.Name;
                }
                if (task.Priority != null)
                {
                    gettask.Priority = task.Priority;
                }
                if (task.StaffId != null && task.StaffId != "")
                {
                    gettask.StaffId = task.StaffId;
                    gettask.ClientId = null;
                }
                if (task.Status != null)
                {
                    gettask.Status = task.Status;
                }
                dbContext.Entry(gettask).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Tasks> UpdateWorkflowTaskAsync(Tasks task)
        {
            try
            {
                var gettask = dbContext.Tasks.ToList().Where(x => x.Id == task.Id).FirstOrDefault();

                if (task.Description != null)
                {
                    gettask.Description = task.Description;
                }
                if (task.Name != null)
                {
                    gettask.Name = task.Name;
                }
                if (task.Priority != null)
                {
                    gettask.Priority = task.Priority;
                }
                if (task.Status != null)
                {
                    gettask.Status = task.Status;
                }
                dbContext.Entry(gettask).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteTaskAsync(int Id)
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
        public async Task ChangeStatusAsync(int Id, string Status)
        {
            try
            {
                Tasks task = await dbContext.Tasks.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    if (Status == "Completed")
                        task.Status = Entities.Status.Completed;
                    else
                        task.Status = Entities.Status.InProgress;

                    dbContext.Entry(task).State = EntityState.Modified;
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
