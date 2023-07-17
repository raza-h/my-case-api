using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class TransactionService : ITransactionService
    {
        private readonly ApiDbContext dbContext;
        public TransactionService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddTransactionAsync(Transaction transaction)
        {
            try
            {
                string newAcode = string.Empty;
                var list = await dbContext.Transaction.ToListAsync();
                if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNumber))
                {
                    var splitedCode = list.LastOrDefault().InvoiceNumber.Split('-');
                    int lastPartOfCode = Convert.ToInt32(splitedCode[2]) + 1;
                    newAcode = $"SI-{DateTime.Now.Year}-000" + Convert.ToString(lastPartOfCode);
                }
                else
                {
                    newAcode = $"SI-{DateTime.Now.Year}-0001";
                }
                transaction.InvoiceNumber = newAcode;

                await dbContext.Transaction.AddAsync(transaction);
                await dbContext.SaveChangesAsync();
                return transaction.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddExpenseTransactionAsync(Transaction transaction)
        {
            try
            {
                string newAcode = string.Empty;
                var list = await dbContext.Transaction.ToListAsync();
                //if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNumber))
                //{
                //    var splitedCode = list.LastOrDefault().InvoiceNumber.Split('-');
                //    int lastPartOfCode = Convert.ToInt32(splitedCode[2]) + 1;
                //    newAcode = $"EI-{DateTime.Now.Year}-000" + Convert.ToString(lastPartOfCode);
                //}
                //else
                //{
                //    newAcode = $"EI-{DateTime.Now.Year}-0001";
                //}
                //transaction.InvoiceNumber = newAcode;

                await dbContext.Transaction.AddAsync(transaction);
                await dbContext.SaveChangesAsync();
                return transaction.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            try
            {
                float openingBalance = 0;
                List<Transaction> transactions = await dbContext.Transaction.ToListAsync();
                if(transactions != null && transactions.Count > 0)
                {
                    for (int i = 0; i < transactions.Count; i++)
                    {
                        transactions[i] = setBalance(transactions[i], openingBalance);
                        openingBalance = float.Parse(transactions[i].ClosingBalance, CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Transaction>> GetTransactionsByDatesAsync(string AccountNum, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                if (DateFrom <= DateTo)
                {
                    transactions = await dbContext.Transaction.Where(x => x.AccountNumber == AccountNum && x.Date >= DateFrom && x.Date <= DateTo).ToListAsync();
                    for (int i = 0; i < transactions.Count(); i++)
                    {
                        if (transactions[i].InvoiceNumber.Contains("EI-"))
                        {
                            var GetExpense = dbContext.Expenses.ToList().Where(x => x.InvoiceNo == transactions[i].InvoiceNumber).FirstOrDefault();
                            if(GetExpense != null)
                            {
                                transactions[i].Description = GetExpense.Description;
                            }
                        }
                    }
                }
                else
                {
                    transactions = await dbContext.Transaction.Where(x => x.AccountNumber == AccountNum && x.Date >= DateTo && x.Date <= DateFrom).ToListAsync();
                }

                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ClientTransaction>> GetByParentIdAsync(string ParentId)
        {
            try
            {
                List<ClientTransaction> clientTransactions = await dbContext.ClientTransaction.Where(x => x.ParentId == ParentId).ToListAsync();
                List<CaseDetail> cases = await dbContext.CaseDetail.ToListAsync();
                if (clientTransactions != null && clientTransactions.Count > 0)
                {
                    List<Contact> contacts = await dbContext.Contact.ToListAsync();
                    List<User> users = await dbContext.User.ToListAsync();

                    foreach (var transaction in clientTransactions)
                    {
                        Contact contact = contacts.Where(x => x.ContactId == transaction.ContactId).FirstOrDefault();
                        User user = users.Where(x => x.Id == transaction.UserId).FirstOrDefault();
                        CaseDetail caseDetail = cases.Where(x => x.BillingContact == contact.ContactId).FirstOrDefault();
                        transaction.Contact = contact;
                        transaction.User = user;
                    }
                }
                return clientTransactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Transaction>> GetCustomersTransactionsAsync()
        {
            try
            {
                List<Transaction> transactions = await dbContext.Transaction.Where(x => x.AccountType == "Customer" || x.AccountType == "Sales").ToListAsync();
                foreach (var item in transactions)
                {
                    var getdata = dbContext.Payment.ToList().Where(x => x.InvoiceNo == item.InvoiceNumber).FirstOrDefault();
                    item.Paymenttype = getdata.PaymentType;
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Transaction>> GetCustomersTransactionsAsyncClient()
        {
            try
            {
                List<Transaction> transactions = await dbContext.Transaction.Where(x => x.AccountType == "Client" || x.AccountType == "Staff").ToListAsync();
                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Transaction> GetByIdAsync(int Id)
        {
            try
            {
                Transaction transaction = await dbContext.Transaction.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Transaction>> GetTransactionByAccountNoAsync(string accountNo, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<Transaction> transaction = await dbContext.Transaction.Where(x => x.AccountNumber == accountNo && x.Date >= DateFrom && x.Date <= DateTo).ToListAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Transaction> GetByInvoiceNoAsync(string invoiceNo)
        {
            try
            {
                Transaction transaction = await dbContext.Transaction.Where(x => x.InvoiceNumber == invoiceNo).FirstOrDefaultAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            try
            {
                dbContext.Entry(transaction).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(int Id)
        {
            Transaction transaction = await dbContext.Transaction.Where(x => x.Id == Id).FirstOrDefaultAsync();
            try
            {
                dbContext.Entry(transaction).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Transaction setBalance(Transaction transaction, float openingBalance)
        {
            try
            {
                if(transaction != null && transaction.transactionType == TransactionType.Credit)
                {
                    transaction.OpeningBalance = openingBalance.ToString();
                    transaction.ClosingBalance = (openingBalance + float.Parse(transaction.Amount, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                }
                else
                {
                    transaction.OpeningBalance = openingBalance.ToString();
                    transaction.ClosingBalance = (openingBalance - float.Parse(transaction.Amount, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                }
                return transaction;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
