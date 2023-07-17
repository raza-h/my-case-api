using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientTransactionController : CommonController
    {
        private readonly IClientTransactionService clientTransactionService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;
        private readonly IContactService contactService;
        private readonly IActivityService activityService;
        private readonly ApiDbContext dbContext;
        private readonly IFinancialDetailsService financialDetailsService;
        public ClientTransactionController(IContactService contactService, IWebHostEnvironment env, IClientTransactionService clientTransactionService, ITransactionService transactionService, UserManager<User> userManager, IActivityService activityService, IFinancialDetailsService financialDetailsService, ApiDbContext dbContext) : base(env)
        {
            this.clientTransactionService = clientTransactionService;
            this.userManager = userManager;
            this.contactService = contactService;
            this.activityService = activityService;
            this.transactionService = transactionService;
            this.financialDetailsService = financialDetailsService;
            this.dbContext = dbContext;

        }
        [HttpPost]
        [Route("AddClientTransaction")]
        public async Task<IActionResult> AddClientTransaction(ClientTransaction clientTransaction)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                clientTransaction.ParentId = loggedInUser != null ? loggedInUser.Id : null;

                if (clientTransaction.File != null && clientTransaction.File.Length > 0)
                {
                    var imgPath = SaveImage(clientTransaction.File, Guid.NewGuid().ToString());
                    clientTransaction.File = null;
                    clientTransaction.CheckImagePath = imgPath;
                }
                int Id = await clientTransactionService.AddClientTransactionAsync(clientTransaction);
                if (Id > 0)
                {
                    Transaction transaction = await SetTransactionData(clientTransaction);
                    await transactionService.AddTransactionAsync(transaction);
                    await activityService.AddActivity("Add ClientTransaction", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} paid {clientTransaction.Amount} amount", loggedInUser.Id);
                    return Ok("Client transaction added successfully");
                }
                else
                    return BadRequest("Error while adding client transaction");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateClientTransaction")]
        public async Task<IActionResult> UpdateClientTransaction(ClientTransaction clientTransaction)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                if (clientTransaction.File != null && clientTransaction.File.Length > 0)
                {
                    var imgPath = SaveImage(clientTransaction.File, Guid.NewGuid().ToString());
                    clientTransaction.File = null;
                    clientTransaction.CheckImagePath = imgPath;
                }
                clientTransaction = await clientTransactionService.UpdateAsync(clientTransaction);
                await activityService.AddActivity("Update ClientTransaction", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated paid amount", loggedInUser.Id);
                return Ok(clientTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions(string ParentId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<ClientTransaction> transactions = await clientTransactionService.GetAsync(ParentId);
                        return Ok(transactions);
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTransactionsByParentId")]
        public async Task<IActionResult> GetTransactionsByParentId(string ParentId)
        {
            try
            {
                List<ClientTransaction> transactions = await clientTransactionService.GetByUserIdAsync(ParentId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTransactionByParentId")]
        public async Task<IActionResult> GetTransactionByParentId(string ParentId)
        {
            try
            {
                List<ClientTransaction> transactions = await clientTransactionService.GetByParentIdAsync(ParentId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTransactionsByUserId")]
        public async Task<IActionResult> GetTransactionsByUserId(string userId)
        {
            try
            {
                List<ClientTransaction> transactions = await clientTransactionService.GetByParentIdAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTransactionById")]
        public async Task<IActionResult> GetTransactionById(int Id)
        {
            try
            {
                ClientTransaction transactions = await clientTransactionService.GetByIdAsync(Id);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                List<Contact> _resultModel = await contactService.GetContactsAndCaseName();
                return Ok(_resultModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInvoiceNum")]
        public async Task<IActionResult> GetInvoiceNum()
        {
            try
            {
                string InvoiceNum = await clientTransactionService.GetInvoiceNum();
                return Ok(InvoiceNum);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetClientLastPaidAmount")]
        public async Task<IActionResult> GetClientLastPaidAmount(int clientId)
        {
            try
            {
                string ClientLastPaidAmount = await clientTransactionService.GetClientLastPaidAmount(clientId);
                return Ok(ClientLastPaidAmount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<Transaction> SetTransactionData(ClientTransaction clientTransaction)
        {
            Contact contact = await contactService.GetByIdAsync(clientTransaction.ContactId.Value);
            FinancialDetails financialDetails = await financialDetailsService.GetFinancialDetailByUserIdAsync(contact.UserId);
            Transaction transaction = new Transaction();
            if (clientTransaction != null && financialDetails != null)
            {
                transaction.Amount = clientTransaction.Amount;
                transaction.InvoiceNumber = clientTransaction.InvoiceNo;
                transaction.Date = DateTime.Now;
                transaction.AccountType = Usertype.Client.ToString();
                transaction.AccountNumber = financialDetails.AccountNumber;
                transaction.AccountName = financialDetails.Name;
                transaction.transactionType = TransactionType.Credit;
            }
            return transaction;
        }
    }
}
