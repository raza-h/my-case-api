using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class ExpenseServices : IExpenseService
    {
        private readonly ApiDbContext dbContext;
        public ExpenseServices(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddExpense(Expense model)
        {
            try
            {

                    await dbContext.Expenses.AddAsync(model);
                    await dbContext.SaveChangesAsync();
                    return model.Id;
          
          
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        public async Task<List<Expense>> GetExpenses()
        {
            List<Expense> expenses = await dbContext.Expenses.ToListAsync();
            return expenses;
        }
        public async Task<int> AddPaymentsType(PaymentTypes model)
        {
            try
            {
             


                    await dbContext.PaymentTypes.AddAsync(model);
                    await dbContext.SaveChangesAsync();
                    return model.Id;
          
          
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PaymentTypes>> GetPaymentTypes()
        {
            try
            {



                List<PaymentTypes> PaymentTypes = await dbContext.PaymentTypes.ToListAsync();
                return PaymentTypes;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetInvoiceNum()
        {
            string InvoiceNum = string.Empty;
            var list = await dbContext.Expenses.ToListAsync();
            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNo))
            {
                var splitedCode = list.LastOrDefault().InvoiceNo.Split('-');
                int lastPartOfCode = Convert.ToInt32(splitedCode[3]) + 1;
                InvoiceNum = $"EI-01-001-000" + Convert.ToString(lastPartOfCode);
            }
            else
                InvoiceNum = "EI-01-001-0001";

            return InvoiceNum;
        }
        public async Task<Expense> GetExpenseByid(int Id)
        {
            try
            {
                Expense model = await dbContext.Expenses.FindAsync(Id);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public async Task<PaymentTypes> GetPaymentByid(int Id)
        {
            try
            {
                PaymentTypes model = await dbContext.PaymentTypes.FindAsync(Id);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Expense>> GetExpenseByParentId(string CreatedBy, string DateFrom, string DateTo)
        {
            try
            {
                DateTime? dateFrom = Convert.ToDateTime(DateFrom);
                DateTime? dateTo = Convert.ToDateTime(DateTo);
                List<Expense> NewExpenseList = new List<Expense>();

           List<Expense> Expense = dbContext.Expenses.Where(x => x.CreatedBy == CreatedBy && (x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo)).ToList();
               
                
              

                return Expense;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
