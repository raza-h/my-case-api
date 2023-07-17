using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ITaskService
    {
        Task<int> AddTaskAsync(Tasks task);
        Task<int> AddWorkflowTaskAsync(Tasks task);
        Task<List<Tasks>> GetTasksAsync(string userId = "", string userType = "");
        Task<Tasks> GetTaskByIdAsync(int Id);
        Task<Tasks> UpdateTaskAsync(Tasks task);
        Task<Tasks> UpdateWorkflowTaskAsync(Tasks task);
        Task DeleteTaskAsync(int Id);
        Task ChangeStatusAsync(int Id, string Status);
    }
}
