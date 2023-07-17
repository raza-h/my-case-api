using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
   public interface IExpenseService
    {
        Task<int> AddExpense(Expense model);
        Task<int> AddPaymentsType(PaymentTypes model);
        Task<List<PaymentTypes>> GetPaymentTypes();
        Task<List<Expense>> GetExpenseByParentId(string CreatedBy, string DateFrom, string DateTo);
        Task<List<Expense>> GetExpenses();
        Task<Expense> GetExpenseByid(int Id);
        Task<string> GetInvoiceNum();
        Task<PaymentTypes> GetPaymentByid(int Id);
    }
}
