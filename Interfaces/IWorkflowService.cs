using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IWorkflowService
    {
        Task<int> AddWorkflowAsync(WorkflowBase task);
        Task<int> AttachWorkflowToCaseAsync(WorkflowAttach data);
        Task<List<WorkflowBase>> GetWorkflowAsync();
        Task<WorkflowBase> GetWorkflowByIdAsync(int Id);
        Task<WorkflowBase> UpdateWorkflowAsync(WorkflowBase task);
        Task DeleteWorkflowAsync(int Id);
    }
}
