using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    //[ServiceFilter(typeof(MyFilter))]
    [ApiController]

    public class RequestUserController : CommonController
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IRequestUserService RequestUserService;
        private readonly IPricePlanService pricePlanService;
        private readonly IMapper mapper;
        private readonly ApiDbContext _context;
        private const string secretKey = "this_is_my_case_secret-Key-for-token_generation";
        public static readonly SymmetricSecurityKey signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public RequestUserController(IWebHostEnvironment env, UserManager<User> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IUserService userService,
                                        IPricePlanService pricePlanService,
                                        IRequestUserService RequestUserService,
                                        SignInManager<User> signInManager,
                                        IMapper mapper,
                                        ApiDbContext context): base(env)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.RequestUserService = RequestUserService;
            this.pricePlanService = pricePlanService;
            this.signInManager = signInManager;
            this.mapper = mapper;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("AddRequestUser")]
      
        public async Task<ActionResult<CaseApiResult<string>>> AddRequestUser(SignupDto signupDto)
        {
            try
            {   
                if (ModelState.IsValid)
                {
                    RequestUsers model = mapper.Map<RequestUsers>(signupDto.UserSignupDto);
                    model.PlanID = signupDto.PricePlanId;
                    model.VerificationStatus = VerificationStatus.Pending;
                    UserDto userDto = mapper.Map<UserDto>(signupDto.UserSignupDto);
                    RequestUsers existingUser = await RequestUserService.GetByEmailAsync(model.Email);
                    PricePlan Priceplan = await pricePlanService.GetPricePlanByIdAsync(signupDto.PricePlanId);

                    if (existingUser == null || (signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount))
                    {
                       

                        if (signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount && signupDto.File != null && signupDto.File.Length > 0)
                            model = existingUser;
                        else
                            model.Id = await RequestUserService.AddRequestUser(model);

                        if (model.Id > 0)
                        {
                            if ((signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal) || (signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount))
                            {
                                UserSubcription userSubcription = SetSubscriptionData(signupDto,Priceplan, Convert.ToString(model.Id));
                                await userService.AddUserSubscription(userSubcription);
                                PaymentInfo paymentInfo = SetPaymentInfo(signupDto, Priceplan, Convert.ToString(model.Id), userSubcription.Id);
                                await userService.AddPaymentInfo(paymentInfo);
                                UserVerification userVerification = new UserVerification
                                {
                                    AdminApproval = signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal ? true : false,
                                    AutoApproval = signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal ? true : false,
                                    PaymentId = paymentInfo.Id,
                                    PaymentType = signupDto.PaymentInfoDto.PaymentType,
                                    UserId =Convert.ToString(model.Id)
                                };
                             

                                await userService.AddUserVerificationAsync(userVerification);
                            }
                            if (signupDto.PaymentInfoDto.PaymentType == PaymentType.None)
                            {
                                SendEmail(signupDto.UserSignupDto.Email);
                                return new CaseApiResult<string>
                                {
                                    Data = string.Empty,
                                    IsSuccess = true,
                                    StatusCode = StatusCodes.Status200OK,
                                    Exception = null
                                };
                            }
                    
                            return new CaseApiResult<string>
                            {
                                Data = JsonConvert.SerializeObject(userDto),
                                IsSuccess = true,
                                StatusCode = StatusCodes.Status200OK,
                                Exception = null
                            };
                        }
                        else
                        {
                            return BadRequest(new CaseApiResult<string>
                            {
                                Data = string.Empty,
                                IsSuccess = false,
                                StatusCode = StatusCodes.Status400BadRequest,
                                ErrorMessage = "Error while adding user",
                                Exception = null,
                            });
                        }
                    }
                    else
                    {
                        {
                            return BadRequest(new CaseApiResult<string>
                            {
                                Data = string.Empty,
                                IsSuccess = false,
                                StatusCode = StatusCodes.Status400BadRequest,
                                ErrorMessage = "Email already exists",
                                Exception = null,
                            });
                        }
                    }

                }
                else
                {
                    return BadRequest(new CaseApiResult<string>
                    {
                        Data = string.Empty,
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = JsonConvert.SerializeObject(ModelState),
                        Exception = null,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new CaseApiResult<string>
                {
                    Data = string.Empty,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Exception = null,
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetRequestUsers")]
        public async Task<IActionResult> GetRequestUsers(string Status = "")
        {
            try
            {
                List<RequestUsers> users = await RequestUserService.GetRequestUser(Status);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetRequestUserById")]
        public async Task<IActionResult> GetRequestUserById(string Id)
        {
            try
            {
                RequestUsers user = await RequestUserService.GetRequestUserById(Id);
                if (user != null)
                    return Ok(user);
                else
                    return NotFound("No record found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(string Id ,string Status)
        {
            try
            {
                await RequestUserService.ChangeStatus(Id ,Status);
           
                    return Ok("Changed Sucessfully");
                
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private UserSubcription SetSubscriptionData(SignupDto signupDto, PricePlan plan, string userId)
        {
            try
            {
                UserSubcription userSubcription = new UserSubcription()
                {
                    UserId = userId,
                    PricePlanId = plan.PlanID,
                    Paymenttype = signupDto.PaymentInfoDto.PaymentType,
                    Amount = plan.PriceRange,
                    PaymentStatus = PaymentStatus.Paid,
                    StartDate = DateTime.Now,
                    EndDate = plan.TimeRange == "Per Week" ? DateTime.Now.AddDays(7) : plan.TimeRange == "Per Month" ? DateTime.Now.AddMonths(1) : DateTime.Now.AddMonths(12),
                    CreationTime = DateTime.Now.TimeOfDay
                };
                return userSubcription;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private PaymentInfo SetPaymentInfo(SignupDto signupDto, PricePlan plan, string userId, int userSubscriptionId)
        {
            try
            {
                PaymentInfo paymentInfo = new PaymentInfo();
                paymentInfo.Email = signupDto.UserSignupDto.Email;
                paymentInfo.InvoiceNo = "0012";
                paymentInfo.PaymentDate = DateTime.Now;
                paymentInfo.PaymentType = signupDto.PaymentInfoDto.PaymentType;
                paymentInfo.PlanName = plan.PlanName;
                paymentInfo.Price = plan.PriceRange;
                paymentInfo.UserId = userId;
                paymentInfo.SubsciptionId = userSubscriptionId;

                return paymentInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
