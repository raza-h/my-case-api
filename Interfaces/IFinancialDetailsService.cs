using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IFinancialDetailsService
    {
        Task<int> AddFinancialDetailsAsync(FinancialDetails financialDetails);
        Task<List<FinancialDetails>> GetFinancialDetailsAsync(string parentId);
        Task<FinancialDetails> GetFinancialDetailByUserIdAsync(string userId);
        Task<FinancialDetails> GetFinancialDetailsStaffAsync(string staffId);
        Task<List<PaymentInfoDto>> GetPaymentsAsync(string UserId, string role = "");
        List<FinancialDetails> GetAllFinancialAccounts();
        Task<List<FinancialDetails>> GetFinancialAccounts();
        Task<PaymentInfoDto> GetPaymentByIdAsync(int PaymentId);
        Task<bool> AddTransactionAsync(Transaction Transactions);
        Task<List<Service>> GetSubsAsync(string UserId);
    }
}
