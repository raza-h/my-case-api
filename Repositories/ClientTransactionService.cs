using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class ClientTransactionService : IClientTransactionService
    {
        private readonly ApiDbContext dbContext;

        public ClientTransactionService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddClientTransactionAsync(ClientTransaction clientTransaction)
        {
            try
            {
                var contact = await dbContext.Contact.Where(x => x.ContactId == clientTransaction.ContactId).FirstOrDefaultAsync();
                if (contact != null)
                    clientTransaction.UserId = contact.UserId;
                string InvoiceNum = string.Empty;
                var list = await dbContext.ClientTransaction.ToListAsync();
                if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNo))
                {
                    var splitedCode = list.LastOrDefault().InvoiceNo.Split('-');
                    int lastPartOfCode = Convert.ToInt32(splitedCode[3]) + 1;
                    InvoiceNum = $"CI-01-001-000" + Convert.ToString(lastPartOfCode);
                }
                else
                    InvoiceNum = $"CI-01-001-0001";

                clientTransaction.InvoiceNo = InvoiceNum;
                clientTransaction.PaymentDate = DateTime.Now.Date;
                clientTransaction.CheckDate = clientTransaction.CheckDate != null ? clientTransaction.CheckDate.Value.Date : null;
                await dbContext.ClientTransaction.AddAsync(clientTransaction);
                await dbContext.SaveChangesAsync();
                return clientTransaction.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(int Id)
        {
            try
            {
                ClientTransaction clientTransaction = await dbContext.ClientTransaction.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(clientTransaction).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ClientTransaction>> GetAsync(string ParentId)
        {
            try
            {
                List<ClientTransaction> clientTransactions = await dbContext.ClientTransaction.Where(x => x.ParentId == ParentId).ToListAsync();
                if (clientTransactions != null && clientTransactions.Count > 0)
                {
                    List<Contact> contacts = await dbContext.Contact.ToListAsync();
                    foreach (var transaction in clientTransactions)
                    {
                        Contact contact = contacts.Where(x => x.ContactId == transaction.ContactId).FirstOrDefault();
                        transaction.ClientName = contact != null ? $"{contact.FirstName} {contact.LastName}" : "N/A";
                    }
                }
                return clientTransactions;
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
                List<ClientTransaction> _newclientTransactions = new List<ClientTransaction>();
                if (clientTransactions != null && clientTransactions.Count > 0)
                {
                    List<Contact> contacts = await dbContext.Contact.ToListAsync();
                    List<User> users = await dbContext.User.ToListAsync();

                    foreach (var transaction in clientTransactions)
                    {
                        ClientTransaction model = new ClientTransaction();

                        model.Amount = transaction.Amount;
                        Contact contact = contacts.Where(x => x.ContactId == transaction.ContactId).FirstOrDefault();
                        User user = users.Where(x => x.ParentId == transaction.ParentId).FirstOrDefault();
                        CaseDetail caseDetail = cases.Where(x => x.BillingContact == contact.ContactId).FirstOrDefault();
                        model.CaseName = caseDetail.CaseName;
                        model.CheckAmount = transaction.CheckAmount;
                        model.CheckDate = transaction.CheckDate;
                        model.CheckImagePath = transaction.CheckImagePath;
                        model.CheckNumber = transaction.CheckNumber;
                        model.CheckTitle = transaction.CheckTitle;
                        model.ClientName = contact.FirstName;
                        model.ContactId = transaction.ContactId;
                        model.Id = transaction.Id;
                        model.InvoiceNo = transaction.InvoiceNo;
                        model.IsCash = transaction.IsCash;
                        model.IsCredit = transaction.IsCredit;
                        model.Note = transaction.Note;
                        model.ParentId = transaction.ParentId;
                        model.PaymentDate = transaction.PaymentDate;
                        model.PaymentType = transaction.PaymentType;
                        model.Contact= contact;
                        model.User= user;

                        _newclientTransactions.Add(model);


                        //User user = users.Where(x => x.Id == transaction.UserId).FirstOrDefault();

                        //transaction.Contact = contact;
                        //transaction.User = user;
                    }
                }
                return _newclientTransactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ClientTransaction>> GetByUserIdAsync(string userId)
        {
            try
            {
                List<ClientTransaction> clientTransactions = await dbContext.ClientTransaction.Where(x => x.UserId == userId).ToListAsync();
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

        public async Task<ClientTransaction> GetByIdAsync(int Id)
        {
            try
            {
                ClientTransaction clientTransaction = await dbContext.ClientTransaction.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return clientTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientTransaction> UpdateAsync(ClientTransaction clientTransaction)
        {
            try
            {
                dbContext.Entry(clientTransaction).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return clientTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetInvoiceNum()
        {
            string InvoiceNum = string.Empty;
            var list = await dbContext.ClientTransaction.ToListAsync();
            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNo))
            {
                var splitedCode = list.LastOrDefault().InvoiceNo.Split('-');
                int lastPartOfCode = Convert.ToInt32(splitedCode[3]) + 1;
                InvoiceNum = $"CI-01-001-000" + Convert.ToString(lastPartOfCode);
            }
            else
                InvoiceNum = "CI-01-001-0001";

            return InvoiceNum;
        }
        public async Task<string> GetClientLastPaidAmount(int clientId)
        {
            try
            {
                string LastPaidAmount = string.Empty;
                var list = await dbContext.ClientTransaction.Where(x => x.ContactId == clientId).ToListAsync();
                if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().Amount))
                    LastPaidAmount = list.LastOrDefault().Amount;
                else
                    LastPaidAmount = "Not paid any amount";

                return LastPaidAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ClientTransaction>> GetTransationByClientId(int clientId, string DateFrom,string DateTo)
        {
            try
            {
                DateTime? dateFrom = Convert.ToDateTime(DateFrom);
                DateTime? dateTo = Convert.ToDateTime(DateTo);
                List<ClientTransaction> _resultModel = new List<ClientTransaction>();

                List<ClientTransaction> clientTransaction = await dbContext.ClientTransaction.Where(x => x.ContactId == clientId && (x.PaymentDate >= dateFrom && x.PaymentDate <= dateTo)).ToListAsync();
                foreach (var item in clientTransaction)
                {
                    ClientTransaction model = new ClientTransaction();
                    model.Amount = item.Amount;
                    model.CaseName = item.CaseName;
                    model.CheckAmount = item.CheckAmount;
                    model.CheckDate = item.CheckDate;
                    model.ClientName = item.ClientName;
                    model.ContactId = item.ContactId;
                    model.InvoiceNo = item.InvoiceNo;
                    model.IsCash = item.IsCash;
                    model.IsCredit = item.IsCredit;
                    model.Note = item.Note;
                    model.ParentId = item.ParentId;
                    model.CreatedDate = item.PaymentDate.Value.ToString("dd-MM-yyyy");
                    model.PaymentType = item.PaymentType;

                }

                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
