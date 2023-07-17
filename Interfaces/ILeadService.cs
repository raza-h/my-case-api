using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ILeadService
    {
        Task<int> AddLeadAsync(Lead lead);
        Task<List<Lead>> GetLeadsAsync();
        Task<Lead> GetLeadByIdAsync(int Id);
        Task<Lead> UpdateLeadAsync(Lead lead);
        Task DeleteLeadAsync(int Id);
    }
}
