using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialDetailsController : ControllerBase
    {
        private readonly IFinancialDetailsService financialService;
        private readonly UserManager<User> userManager;
        private readonly ApiDbContext dbContext;
        public FinancialDetailsController (IFinancialDetailsService financialService, UserManager<User> userManager, ApiDbContext dbContext)
        {
            this.financialService = financialService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetFinancialDetails")]
        public async Task<IActionResult> GetFinancialDetails()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                List<FinancialDetails> financialDetails = await financialService.GetFinancialDetailsAsync(loggedInUser != null ? loggedInUser.Id : string.Empty);
                return Ok(financialDetails);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetAccountById")]
        public async Task<IActionResult> GetAccountById(string Id)
        {
            try
            {
                FinancialDetails financialDetails = await financialService.GetFinancialDetailsStaffAsync(Id);
                return Ok(financialDetails);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetPayments")]
        public async Task<IActionResult> GetPayments()
        {
            try
            {
                string email = User.FindFirstValue(ClaimTypes.Email);
                string role = User.FindFirstValue(ClaimTypes.Role);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                List<PaymentInfoDto> payments = await financialService.GetPaymentsAsync(loggedInUser != null ? loggedInUser.Id : string.Empty, role);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("PaymentDetail")]
        public async Task<IActionResult> PaymentDetail(int paymentId)
        {
            try
            {
                PaymentInfoDto payment = await financialService.GetPaymentByIdAsync(paymentId);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetSubscriptionNames")]
        public async Task<IActionResult> GetSubscriptionNames()
        {
            try
            {
                //string email = "attorneyone@gmail.com";
                string email = User.FindFirstValue(ClaimTypes.Email);
                string role = User.FindFirstValue(ClaimTypes.Role);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                List<Service> subs = await financialService.GetSubsAsync(loggedInUser != null ? loggedInUser.Id : string.Empty);
                return Ok(subs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
