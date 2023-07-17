using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ITransactionService
    {
        Task<int> AddTransactionAsync(Transaction transaction);
        Task<int> AddExpenseTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsAsync();
        Task<List<Transaction>> GetTransactionsByDatesAsync(string AccountNum, DateTime DateFrom, DateTime DateTo);
        Task<List<Transaction>> GetCustomersTransactionsAsync();
        Task<List<Transaction>> GetCustomersTransactionsAsyncClient();
        Task<List<ClientTransaction>> GetByParentIdAsync(string ParentId);
        Task<Transaction> GetByIdAsync(int Id);
        Task<List<Transaction>> GetTransactionByAccountNoAsync(string accountNo, DateTime DateFrom, DateTime DateTo);
        Task<Transaction> GetByInvoiceNoAsync(string invoiceNo);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task DeleteAsync(int Id);
    }
}
