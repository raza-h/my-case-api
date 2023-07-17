using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IClientTransactionService
    {
        Task<int> AddClientTransactionAsync(ClientTransaction clientTransaction);
        Task<List<ClientTransaction>> GetAsync(string ParentId);
        Task<List<ClientTransaction>> GetByParentIdAsync(string ParentId);
       
        Task<List<ClientTransaction>> GetByUserIdAsync(string userId);
        Task<ClientTransaction> GetByIdAsync(int Id);
        Task<ClientTransaction> UpdateAsync(ClientTransaction clientTransaction);
        Task<List<ClientTransaction>> GetTransationByClientId(int clientId, string DateFrom, string DateTo);
        Task DeleteAsync(int Id);
        Task<string> GetInvoiceNum();
        Task<string> GetClientLastPaidAmount(int clientId);
    }
}
