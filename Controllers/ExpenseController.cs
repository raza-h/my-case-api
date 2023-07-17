using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : CommonController
    {
        private readonly ICaseService caseService;
        private readonly IFinancialDetailsService financialService;
        private readonly IExpenseService expenseService;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly IContactService contactService;
        private readonly IActivityService activityService;
        private readonly ITransactionService transactionService;
        private readonly ApiDbContext dbContext;
        public ExpenseController(IUserService userService, ITransactionService transactionService, ICaseService caseService, IFinancialDetailsService financialService, IExpenseService expenseService, IWebHostEnvironment env, UserManager<User> userManager, IContactService contactService, IActivityService activityService, ApiDbContext dbContext) : base(env)
        {
            this.caseService = caseService;
            this.financialService = financialService;
            this.expenseService = expenseService;
            this.env = env;
            this.userManager = userManager;
            this.transactionService = transactionService;
            this.userService = userService;
            this.activityService = activityService;
            this.contactService = contactService;
            this.dbContext = dbContext;

        }

        [HttpGet]
        [Route("GetInvoiceNum")]
        public async Task<IActionResult> GetInvoiceNum()
        {
            try
            {
                string InvoiceNum = await expenseService.GetInvoiceNum();
                return Ok(InvoiceNum);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      
        [HttpPost]
        [Route("AddExpense")]

        public async Task<IActionResult> AddExpense(Expense model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {
                        if(model.ClientId == null || model.ClientId == "" || model.ClientId == "undefined")
                        {
                            return BadRequest("Client is required.");
                        }
                        model.FirmId = GetFirm.Id;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = loggedInUser.FirstName;
                        string InvoiceNum = await expenseService.GetInvoiceNum();
                        if (InvoiceNum != null && InvoiceNum != "")
                            model.InvoiceNo = InvoiceNum;
                        else
                            model.InvoiceNo = "EI-01-001-0001";

                        if (model.File != null && model.File.Length > 0)
                        {
                            var imgPath = SaveImage(model.File, Guid.NewGuid().ToString());
                            model.File = null;
                            model.CheckImagePath = imgPath;
                        }
                        var expense = await expenseService.AddExpense(model);
                        if (expense > 0)
                        {
                            Contact contact = await contactService.GetByIdAsync(Convert.ToInt32(model.ClientId));
                            Transaction transaction = await SetTransactionData(model, contact);
                            await transactionService.AddExpenseTransactionAsync(transaction);
                            await activityService.AddActivity("Add Expense", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added amount {model.Amount} as expense for client {contact.FirstName}", loggedInUser.Id);
                        }
                        return Ok("Expense added successfully");
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AddPaymentTypes")]

        public async Task<int> AddPaymentTypes(PaymentTypes model)
        {
            try
            {

                var paymentType = await expenseService.AddPaymentsType(model);
                return paymentType;
            }
            catch (Exception)
            {

                throw;
            }


        }
        [HttpGet]
        [Route("GetPaymentTypes")]
        public async Task<IActionResult> GetPaymentTypes()
        {
            try
            {

                List<PaymentTypes> PaymentTypes = await expenseService.GetPaymentTypes();
                return Ok(PaymentTypes);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetExpenses")]
        public async Task<IActionResult> GetExpenses()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<Expense> expenses = await expenseService.GetExpenses();
                        List<ExpenseViewModel> _resultModel = new List<ExpenseViewModel>();
                        foreach (var _item in expenses)
                        {
                            ExpenseViewModel model = new ExpenseViewModel();
                            model.Id = _item.Id;
                            model.Amount = _item.Amount;
                            model.CaseId = _item.CaseId;
                            model.FirmId = _item.FirmId;
                            model.ClientId = _item.ClientId;
                            //User user = await userService.GetUserByIdAsync(_item.ClientId);
                            //if (user != null)
                            //    model.ClientName = user.FirstName;
                            model.CheckDate = _item.CheckDate;
                            model.CheckImagePath = _item.CheckImagePath;
                            model.CheckNumber = _item.CheckNumber;
                            model.CheckTitle = _item.CheckTitle;
                            model.CreatedBy = _item.CreatedBy;
                            model.InvoiceNo = _item.InvoiceNo;
                            model.IsCash = _item.IsCash;
                            model.IsCredit = _item.IsCredit;
                            model.ExpenseType = _item.ExpenseType;
                            PaymentTypes paymentType = await expenseService.GetPaymentByid(Convert.ToInt32(_item.ExpenseType));
                            Transaction transaction = await transactionService.GetByInvoiceNoAsync(_item.InvoiceNo);
                            model.ExpenseTypeName = paymentType.Name;
                            model.PaymentMode = _item.PaymentMode;
                            if(transaction!=null)
                            {
                                if (transaction.AccountName!=null && transaction.AccountName!="")
                                {
                                    model.ClientName = transaction.AccountName;
                                }
                            }
                            model.Description = _item.Description;
                            model.CreatedDate = _item.CreatedDate.Value.ToString("dd-MM-yyyy");
                            _resultModel.Add(model);
                        }
                        return Ok(_resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetExpenseById")]
        public async Task<IActionResult> GetExpenseById(int Id)
        {
            try
            {
                var _entity = await expenseService.GetExpenseByid(Id);
                ExpenseViewModel model = new ExpenseViewModel();
                model.Id = _entity.Id;
                model.Amount = _entity.Amount;
                model.CaseId = _entity.CaseId;
                var Case = await caseService.GetCasesById(_entity.CaseId);
                if (Case != null)
                    model.CaseName = Case.CaseName;
                model.ClientId = _entity.ClientId;
                User user = await userService.GetUserByIdAsync(_entity.ClientId);
                if (user != null)
                    model.ClientName = user.FirstName + " " + user.LastName;
                model.CheckDate = _entity.CheckDate;
                model.CheckImagePath = _entity.CheckImagePath;
                model.CheckNumber = _entity.CheckNumber;
                model.CheckTitle = _entity.CheckTitle;
                model.CreatedBy = _entity.CreatedBy;
                model.InvoiceNo = _entity.InvoiceNo;
                model.IsCash = _entity.IsCash;
                model.IsCredit = _entity.IsCredit;
                model.ExpenseType = _entity.ExpenseType;
                model.PaymentMode = _entity.PaymentMode;
                model.Description = _entity.Description;
                model.CreatedDate=_entity.CreatedDate.Value.ToString("dd-MM-yyyy");


                return Ok(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<Transaction> SetExpenseTransaction(Expense expense, FinancialDetails financialDetails)
        {
            try
            {

                var getAccount = financialService.GetAllFinancialAccounts().ToList().Where(x => x.Type == Usertype.Expense).FirstOrDefault();

                List<Transaction> listdata = new List<Transaction>();
                Transaction data = new Transaction();
                data.AccountName = financialDetails.Name;
                data.AccountNumber = financialDetails.AccountNumber;
                data.AccountType = Usertype.Expense.ToString();
                data.DetailAccountId = expense.Id;
                data.Date = DateTime.Now;
                //data.Credit = expense.Amount.ToString();
                //data.Debit = "0.00";
                if (getAccount == null)
                    data.ClosingBalance = expense.Amount.ToString();
                data.InvoiceNumber = expense.InvoiceNo;
                listdata.Add(data);

                getAccount = financialService.GetAllFinancialAccounts().ToList().Where(x => x.Type == Usertype.CashInHand).FirstOrDefault();

                if (getAccount != null)
                {
                    Transaction sdata = new Transaction();
                    sdata.AccountName = getAccount.Name;
                    sdata.AccountNumber = getAccount.AccountNumber;
                    sdata.AccountType = getAccount.Type.ToString();
                    //sdata.InvoiceNumber = pdata.InvoiceNo;
                    data.DetailAccountId = getAccount.Id;
                    sdata.Date = DateTime.Now;
                    //sdata.Credit = "0.00";
                    //sdata.Debit = expense.Amount.ToString();
                    //sdata.ClosingBalance = (Convert.ToDouble(data.Debit) - Convert.ToDouble(data.Credit)).ToString();

                    sdata.InvoiceNumber = expense.InvoiceNo;

                    listdata.Add(sdata);
                }
                return listdata;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("GetExpensesByDates")]
        public async Task<IActionResult> GetExpensesByDates(string CreatedBy, string DateFrom, string DateTo)
        {
            try
            {

                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                if (ParentId != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == ParentId).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<Expense> Expenses = await expenseService.GetExpenseByParentId(CreatedBy, DateFrom, DateTo);
                        List<ExpenseViewModel> _resultModel = new List<ExpenseViewModel>();
                        foreach (var item in Expenses)
                        {
                            ExpenseViewModel model = new ExpenseViewModel();
                            model.Id = item.Id;
                            model.Amount = item.Amount;
                            model.CheckDate = item.CheckDate;
                            model.CheckImagePath = item.CheckImagePath;
                            model.CheckNumber = item.CheckNumber;
                            model.CheckTitle = item.CheckTitle;
                            model.ClientId = item.ClientId;
                            User user = await userService.GetUserByIdAsync(item.ClientId);
                            if (user != null)
                                model.ClientName = user.FirstName + " " + user.LastName;
                            model.CheckDate = item.CheckDate;
                            model.CheckImagePath = item.CheckImagePath;
                            model.CheckNumber = item.CheckNumber;
                            model.CheckTitle = item.CheckTitle;
                            model.CreatedBy = item.CreatedBy;
                            model.InvoiceNo = item.InvoiceNo;
                            model.IsCash = item.IsCash;
                            model.IsCredit = item.IsCredit;
                            model.ExpenseType = item.ExpenseType;
                            PaymentTypes paymentType = await expenseService.GetPaymentByid(Convert.ToInt32(item.ExpenseType));
                            model.ExpenseTypeName = paymentType.Name;
                            model.PaymentMode = item.PaymentMode;
                            model.Description = item.Description;
                            model.CreatedDate = item.CreatedDate.Value.ToString("dd-MM-yyyy");
                            _resultModel.Add(model);

                        }
                        return Ok(_resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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

        private async Task<Transaction> SetTransactionData(Expense expense, Contact contact)
        {
            FinancialDetails financialDetails = await financialService.GetFinancialDetailByUserIdAsync(contact.UserId);
            Transaction transaction = new Transaction();
            if (expense != null && financialDetails != null)
            {
                transaction.Amount = expense.Amount.ToString();
                transaction.InvoiceNumber = expense.InvoiceNo;
                transaction.Date = DateTime.Now;
                transaction.AccountType = Usertype.Client.ToString();
                transaction.AccountNumber = financialDetails.AccountNumber;
                transaction.AccountName = financialDetails.Name;
                transaction.transactionType = TransactionType.Debit;
            }
            return transaction;
        }
    }
}
