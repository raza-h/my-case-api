using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        public TransactionController (ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }
        [HttpPost]
        [Route("AddTransaction")]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            try
            {
                int Id = await transactionService.AddTransactionAsync(transaction);
                if (Id > 0)
                    return Ok("Transaction Added successfully");
                else
                    return BadRequest("something went wrong while adding transaction");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                List<Transaction> transactions = await transactionService.GetTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("GetTransactionsBetweenDates")]
        public async Task<IActionResult> GetTransactionsBetweenDates(string AccountNum, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<Transaction> transactions = await transactionService.GetTransactionsByDatesAsync(AccountNum, DateFrom, DateTo);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCustomersTransactions")]
        public async Task<IActionResult> GetCustomersTransactions()
        {
            try
            {
                List<Transaction> transactions = await transactionService.GetCustomersTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCustomersTransactionsClient")]
        public async Task<IActionResult> GetCustomersTransactionsClient()
        {
            try
            {
                List<Transaction> transactions = await transactionService.GetCustomersTransactionsAsyncClient();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                Transaction transaction = await transactionService.GetByIdAsync(Id);
                if (transaction != null)
                    return Ok(transaction);
                else
                    return NotFound("transaction not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Transaction transaction)
        {
            try
            {
                transaction = await transactionService.UpdateAsync(transaction);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await transactionService.DeleteAsync(Id);
                return Ok("transaction Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTransactionByAccountNo")]
        public async Task<IActionResult> GetTransactionByAccountNo(string accountNo, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<Transaction> transaction = await transactionService.GetTransactionByAccountNoAsync(accountNo, DateFrom, DateTo);
                if (transaction != null)
                    return Ok(transaction);
                else
                    return NotFound("transaction not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
