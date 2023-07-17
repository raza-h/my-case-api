using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Models;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class UserManagementController : CommonController
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IFinancialDetailsService financialService;
        private readonly EmailService emailService;
        private const string secretKey = "this_is_my_case_secret-Key-for-token_generation";
        private readonly IConfiguration config;
        public static readonly SymmetricSecurityKey signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        private readonly ApiDbContext dbContext;
        private readonly IActivityService activityService;

        public UserManagementController(IWebHostEnvironment env, UserManager<User> userManager,

                                             RoleManager<IdentityRole> roleManager,
                                             IUserService userService,
                                             SignInManager<User> signInManager,
                                             IMapper mapper,
                                             ApiDbContext dbContext,
                                             IFinancialDetailsService financialService,
                                             EmailService emailService, IConfiguration config,
                                             IActivityService activityService) : base(env)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.financialService = financialService;
            this.emailService = emailService;
            this.dbContext = dbContext;
            this.config = config;
            this.activityService = activityService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(SignupDto signupDto)
        {
            try
            {
                if (ModelState.IsValid && signupDto != null)
                {
                    User model = mapper.Map<User>(signupDto.UserSignupDto);
                    if (signupDto.PaymentInfoDto != null && signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal)
                        model.VerificationStatus = VerificationStatus.Approved;
                    UserDto userDto = mapper.Map<UserDto>(signupDto.UserSignupDto);
                    User existingUser = await userService.GetByEmailAsync(model.Email);
                    if (existingUser == null || ((existingUser != null && existingUser.RoleName == "Attorney" && signupDto.PaymentInfoDto != null && signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount && signupDto.File != null && signupDto.File.Length > 0)))
                    {
                        if (model.File != null && model.File.Length > 0)
                        {
                            var imgPath = SaveImage(model.File, Guid.NewGuid().ToString());
                            model.File = null;
                            model.ImagePath = imgPath;
                        }

                        if (signupDto.PaymentInfoDto != null && signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount && signupDto.File != null && signupDto.File.Length > 0)
                            model = existingUser;
                        else
                            model.VerificationStatus= VerificationStatus.Pending;
                            model.Id = await userService.AddAsync(model);

                        if (!string.IsNullOrEmpty(model.Id))
                        {
                            var getCurrentUser = dbContext.User.Find(model.Id);
                            getCurrentUser.ParentId = model.Id;
                            dbContext.User.Update(getCurrentUser);
                            dbContext.SaveChanges();
                            if ((signupDto.PaymentInfoDto != null && signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal) || (signupDto.PaymentInfoDto != null && signupDto.PaymentInfoDto.PaymentType == PaymentType.BankAccount && signupDto.File != null && signupDto.File.Length > 0))
                            {
                                UserSubcription userSubcription = SetSubscriptionData(signupDto, signupDto.PricePlan, model.Id);
                                await userService.AddUserSubscription(userSubcription);
                                PaymentInfo paymentInfo = SetPaymentInfo(signupDto, signupDto.PricePlan, model.Id, userSubcription.Id);
                                await userService.AddPaymentInfo(paymentInfo);
                                UserVerification userVerification = new UserVerification
                                {
                                    AdminApproval = signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal ? true : false,
                                    AutoApproval = signupDto.PaymentInfoDto.PaymentType == PaymentType.Paypal ? true : false,
                                    PaymentId = paymentInfo.Id,
                                    PaymentType = signupDto.PaymentInfoDto.PaymentType,
                                    UserId = model.Id
                                };
                                await userService.AddUserVerificationAsync(userVerification);
                                FinancialDetails financialDetails = new FinancialDetails
                                {
                                    Name = $"{userDto.FirstName} {userDto.LastName}",
                                    UserId = model.Id,
                                    Type = userDto.RoleName == "Attorney" ? Usertype.Customer : userDto.RoleName == "Customer" ? Usertype.Customer : userDto.RoleName == "Client" ? Usertype.Client : Usertype.Staff
                                };
                                await financialService.AddFinancialDetailsAsync(financialDetails);
                                List<Transaction> maketransaction = SetCustomerTransaction(financialDetails, paymentInfo);
                                for (int i = 0; i < maketransaction.Count(); i++)
                                {
                                    await financialService.AddTransactionAsync(maketransaction[i]);
                                    if (maketransaction[i].AccountType == "Customer")
                                    {

                                        Transaction transaction = new Transaction();
                                        var GetSaleAcc = financialService.GetAllFinancialAccounts().Where(x => x.Type == Usertype.Sales).FirstOrDefault();
                                        if (GetSaleAcc != null)
                                        {
                                            transaction.AccountType = "Sales";
                                            transaction.Amount = paymentInfo.Price;
                                            transaction.InvoiceNumber = paymentInfo.InvoiceNo;
                                            transaction.DetailAccountId = GetSaleAcc.Id;
                                            transaction.AccountNumber = GetSaleAcc.AccountNumber;
                                            transaction.AccountName = GetSaleAcc.Name;
                                            transaction.Date = DateTime.Now;
                                            transaction.transactionType = TransactionType.Debit;

                                           await financialService.AddTransactionAsync(transaction);

                                        }
                                        else
                                        {

                                            FinancialDetails objAcc = new FinancialDetails();
                                            objAcc.Name = "Sales";
                                            objAcc.Type = Usertype.Sales;
                                            objAcc.AccountNumber = "01-001-0004-0001";
                                            var AccResult = await financialService.AddFinancialDetailsAsync(objAcc);


                                            transaction.AccountType = "Sales";
                                            transaction.Amount = paymentInfo.Price;
                                            transaction.InvoiceNumber = paymentInfo.InvoiceNo;
                                            transaction.DetailAccountId = AccResult;
                                            transaction.AccountNumber = objAcc.AccountNumber;
                                            transaction.AccountName = objAcc.Name;
                                            transaction.Date = DateTime.Now;
                                            transaction.transactionType = TransactionType.Debit;

                                            await financialService.AddTransactionAsync(transaction);
                                        }
                                    }
                                }
                            }
                            string loginwebUrl = config.GetValue<string>("loginurl");
                            bool isEmailSent = emailService.SendWelcomeEmail(userDto.FirstName, "", loginwebUrl);
                            return Ok("User registered successfully");
                        }
                        else
                            return BadRequest("Error while adding user");
                    }
                    else
                        return BadRequest("User with given email already exist");
                }
                else
                    return BadRequest("validation erros");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpPost]
        [Route("InviteMember")]
        public async Task<IActionResult> InviteMember(UserSignupDto userDto)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                User existingUser = await userService.GetByEmailAsync(userDto.Email);
                if (existingUser == null)
                {
                    User model = mapper.Map<User>(userDto);
                    model.CaseRate = userDto.Rate;
                    model.ParentId = loggedInUser != null ? loggedInUser.Id : null;

                    if (userDto.file != null && userDto.file.Length > 0)
                    {
                        var imgPath = SaveImage(userDto.file, Guid.NewGuid().ToString());
                        userDto.file = null;
                        model.ImagePath = imgPath;
                    }
                    model.Password = model.Email;
                    string userId = await userService.AddAsync(model);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        //string webUrl = config.GetValue<string>("webbaseurl");
                        //string url = $"{webUrl}Security/Account/ResetPassword?email={userDto.Email}";
                        //bool isEmailSent = emailService.SendEmail(userDto.Email, loggedInUser != null ? loggedInUser.FirstName : string.Empty, url);
                        FinancialDetails financialDetails = new FinancialDetails
                        {
                            Name = $"{userDto.FirstName} {userDto.LastName}",
                            ParentId = loggedInUser.Id,
                            UserId = userId,
                            Type = userDto.RoleName == "Attorney" ? Usertype.Customer : userDto.RoleName == "Client" ? Usertype.Client : Usertype.Staff
                        };
                        await activityService.AddActivity("Add User", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} invited {userDto.FirstName} newuser", loggedInUser.Id);
                        await financialService.AddFinancialDetailsAsync(financialDetails);
                        return Ok("User invited successfully");
                    }
                    else
                        return BadRequest("Error while inviting user");
                }
                else
                    return BadRequest("User with given email already exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(SignInDto signInDto)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync
                    (
                        userName: signInDto.Email,
                        password: signInDto.Password,
                        isPersistent: true,
                        lockoutOnFailure: false
                    );
                if (result.Succeeded)
                {
                    var user = await userService.GetByEmailAsync(signInDto.Email);
                    UserDto userDto = mapper.Map<UserDto>(user);
                    List<UserSubcription> userSubcriptions = new List<UserSubcription>();
                    if (user.RoleName != "Admin")
                    {
                        string userId = !string.IsNullOrEmpty(user.ParentId) ? user.ParentId : user.Id;
                        userSubcriptions = await dbContext.UserSubcription.Where(x => x.UserId == userId).ToListAsync();
                    }
                    if (user.Status == false)
                        return BadRequest("This account has been blocked, Please contact authorities to get unblocked");

                    userDto.Token = GenerateToken(userDto);
                    // add activity
                    await activityService.AddActivity("Login", $"{user.FirstName} {(!string.IsNullOrEmpty(user.LastName) ? user.LastName : "")} logged in", user.Id);
                    if (user.RoleName != "Admin")
                        userDto = await SetServices(userDto, userSubcriptions);
                    return Ok(userDto);
                }
                else
                    return BadRequest("Username or password is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SubscribePackage")]
        public async Task<IActionResult> SubscribePackage(int planId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                PricePlan plan = await dbContext.PricePlan.Where(x => x.PlanID == planId).FirstOrDefaultAsync();
                UserSubcription userSubcription = new UserSubcription()
                {
                    UserId = loggedInUser.Id,
                    PricePlanId = plan.PlanID,
                    Paymenttype = PaymentType.Paypal,
                    Amount = plan.PriceRange,
                    PaymentStatus = PaymentStatus.Paid,
                    StartDate = DateTime.Now,
                    EndDate = plan.TimeRange == "Per Week" ? DateTime.Now.AddDays(7) : plan.TimeRange == "Per Month" ? DateTime.Now.AddMonths(1) : DateTime.Now.AddMonths(12),
                    CreationTime = DateTime.Now.TimeOfDay
                };
                await userService.AddUserSubscription(userSubcription);
                return Ok("package upgraded successfully");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddUsersBulk")]
        public async Task<IActionResult> AddUsersBulk(List<User> users)
        {
            try
            {
                List<string> Emails = await userService.AddUsersBulk(users);
                return Ok(Emails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            try
            {
                User existingUser = await userService.GetByEmailAsync(Email);

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<User> users = await userService.GetUsersAsync(Status, getCurrentParent.Id);
                        return Ok(users.ToList().Where(x => x.ParentId == getCurrentParent.Id && x.RoleName == "Attorney").ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<User> users = new List<User>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<User> users = await userService.GetAllUsersAsync(getCurrentParent.Id);
                        return Ok(users.ToList().Where(x => x.ParentId == getCurrentParent.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<User> users = new List<User>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpGet]
        [Route("GetUsersMessage")]
        public async Task<IActionResult> GetUsersMessage(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<User> users = await userService.GetUsersAsync(Status, getCurrentParent.Id);
                        return Ok(users.ToList().Where(x => x.ParentId == getCurrentParent.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<User> users = new List<User>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStaff")]
        public async Task<IActionResult> GetStaff(string ParentId, string userType = "")
        {
            try
            {
                List<User> staff = new List<User>();
                List<User> users = await userService.GetUsersAsync("", ParentId);
                if (userType == "Client")
                    staff = users.ToList().Where(x => x.RoleName == "Client").ToList();
                else
                    staff = users.ToList().Where(x => x.RoleName == "Staff").ToList();
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetClientAccounts")]
        public async Task<IActionResult> GetClientAccounts(string ParentId = "")
        {
            try
            {
                List<User> users = await userService.GetUsersAsync("", ParentId);
                List<User> ClientUser = users.Where(x => x.RoleName == "Client").ToList();
                List<FinancialDetails> financialDetails = new List<FinancialDetails>();
                foreach (var item in ClientUser)
                {
                    FinancialDetails financialDetail = await financialService.GetFinancialDetailByUserIdAsync(item.Id);
                    if (financialDetail != null)
                    {
                        financialDetails.Add(financialDetail);
                    }
                }
                return Ok(financialDetails);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetClientsByParentId")]
        public async Task<IActionResult> GetClientsByParentId(string ParentId = "", string userType = "")
        {
            try
            {
                List<User> users = await userService.GetUsersAsync("", ParentId);
                List<User> ClientUser = users.ToList().Where(x => x.RoleName == "Client").ToList();
                return Ok(ClientUser);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [Authorize(Roles = "Attorney,Customer")]
        [HttpGet]
        [Route("GetUserDropDown")]
        public async Task<IActionResult> GetUserDropDown()
        {
            try
            {
                List<UserDropDown> users = await userService.GetUsersWithTitleAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUsersAgainstClientOrStaff")]
        public async Task<IActionResult> GetUsersAgainstClientOrStaff(string userId, string userType)
        {
            try
            {
                List<UserDropDown> users = await userService.GetUsersAgainstClientOrStaffAsync(userId, userType);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                User user = await userService.GetUserByIdAsync(Id);
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

        [HttpGet]
        [Route("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers(string ParentId)
        {
            try
            {
                List<User> users = await userService.GetBlockedUsersAsync(ParentId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [Route("ToggleUserBlock")]
        public async Task<IActionResult> ToggleUserBlock(string UserId, bool Status)
        {
            try
            {
                User user = await userService.GetUserByIdAsync(UserId);
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await userService.ToggleUserBlock(UserId, Status);
                if (!Status)
                {
                    await activityService.AddActivity("Block", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} blocked {(!string.IsNullOrEmpty(user.LastName) ? user.LastName : "")}", loggedInUser.Id);
                    return Ok("User blocked successfully");
                }
                else
                    await activityService.AddActivity("UnBlock", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} unblocked {(!string.IsNullOrEmpty(user.LastName) ? user.LastName : "")}", loggedInUser.Id);
                return Ok("User Unblocked successfully");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User model)
        {
            try
            {
                if (model.File != null && model.File.Length > 0)
                {
                    model.ImagePath = SaveImage(model.File, Guid.NewGuid().ToString());
                    model.File = null;

                }

                model = await userService.UpdateUserAsync(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            try
            {
                await userService.DeleteUserAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(User model, string newPassword)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(model.Email);
                if (currentUser != null)
                {
                    await userManager.RemovePasswordAsync(currentUser);
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        await userManager.AddPasswordAsync(currentUser, newPassword);
                        return Ok("Changed Sucessfully");
                    }
                    else
                    {
                        await userManager.AddPasswordAsync(currentUser, model.newPassword);
                        return Ok();
                    }
                }
                else
                    return NotFound("User Not Found");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Email)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(Email);
                if (currentUser != null)
                {
                    string token = await userManager.GeneratePasswordResetTokenAsync(currentUser);
                    string webUrl = config.GetValue<string>("webbaseurl");
                    string url = $"{webUrl}Security/Account/ResetPassword?email={Email}&&token={token}";
                    bool isEmailSent = emailService.SendEmail(currentUser.Email, "", url);
                    if (isEmailSent)
                        return Ok(token);
                    return BadRequest("An error occured while sending email");
                }
                else
                    return BadRequest("User Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ForgetPassword forgetPassword)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(forgetPassword.Email);
                if (currentUser != null)
                {
                    await userManager.RemovePasswordAsync(currentUser);
                    await userManager.AddPasswordAsync(currentUser, forgetPassword.Password);
                    return Ok("Password changed successfully");
                }
                else
                    return BadRequest("User Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(AspNetRoles model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    IdentityRole existingRole = await roleManager.FindByNameAsync(model.Name);
                    if (existingRole != null)
                        return Content("Role already exist");
                    else
                    {
                        await userService.AddRoleAsync(model.Name);
                        return Ok("Role added successfully");
                    }
                }
                else
                    return Content("Can not create empty role");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await userService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    IdentityRole existingRole = await roleManager.FindByIdAsync(Id);
                    if (existingRole != null)
                    {
                        await userService.DeleteRoleAsync(existingRole);
                        return Ok("Deleted Sucessfully");
                    }
                    else
                        return NotFound("No role found");
                }
                else
                    return NotFound("Provide role id to delete");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(string Id, string Status)
        {
            try
            {
                await userService.ChangeStatus(Id, Status);
                return Ok("Changed Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateToken(UserDto userDto)
        {
            var token = new JwtSecurityToken(
                   claims: new Claim[]
                   {
                       new Claim(ClaimTypes.Email, userDto.Email),
                       new Claim(ClaimTypes.Role, userDto.RoleName),
                       new Claim(ClaimTypes.NameIdentifier, userDto.Id)
                   },
                   notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                   expires: new DateTimeOffset(DateTime.Now.AddHours(5)).DateTime,
                   issuer: userDto.Email,
                   signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
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
        [AllowAnonymous]
        [HttpGet]
        [Route("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin()
        {
            try
            {
                User model = new User()
                {
                    FirstName = "Lydia",
                    LastName = "Charles",
                    Email = "lydiacms00@gmail.com",
                    PhoneNumber = "123456789",
                    Password = "Admin123@",
                    RoleName = "Admin"

                };
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists == null)
                {
                    await userService.AddAsync(model);
                    var getuser = dbContext.User.ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                    model.ParentId = getuser.Id;
                    await userService.UpdateUserAsync(model);
                    return Ok(model);
                }
                else
                    return Ok("User with this email already exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private List<Transaction> SetCustomerTransaction(FinancialDetails fdata, PaymentInfo pdata)
        {
            try
            {
                List<Transaction> listdata = new List<Transaction>();
                Transaction data = new Transaction();
                data.AccountName = fdata.Name;
                data.AccountNumber = fdata.AccountNumber;
                data.AccountType = fdata.Type.ToString();
                data.InvoiceNumber = pdata.InvoiceNo;
                data.Amount = pdata.Price;
                data.DetailAccountId = fdata.Id;
                data.transactionType = TransactionType.Credit;
                data.Date = DateTime.Now;
                listdata.Add(data);

                var getAccount = financialService.GetAllFinancialAccounts().ToList().Where(x => x.Type == Usertype.Attorney).FirstOrDefault();
                if (getAccount != null)
                {
                    Transaction sdata = new Transaction();
                    sdata.AccountName = getAccount.Name;
                    sdata.AccountNumber = getAccount.AccountNumber;
                    sdata.AccountType = getAccount.Type.ToString();
                    sdata.InvoiceNumber = pdata.InvoiceNo;
                    data.DetailAccountId = getAccount.Id;
                    sdata.Date = DateTime.Now;
                }
                return listdata;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task<UserDto> SetServices(UserDto userDto, List<UserSubcription> userSubcriptions)
        {
            try
            {
                List<Service> services = new List<Service>();
                if (userSubcriptions != null && userSubcriptions.Count > 0)
                {
                    List<int> pricePlanIds = userSubcriptions.Where(x => x.EndDate > DateTime.Now).Select(x => x.PricePlanId).ToList();
                    if (pricePlanIds != null && pricePlanIds.Count > 0)
                    {
                        userDto.PricePlanIds = pricePlanIds;
                        var packageService = await dbContext.packageService.Where(x => pricePlanIds.Contains(x.PricePlanId.Value)).Distinct().ToListAsync();
                        if (packageService != null && packageService.Count > 0)
                        {
                            int?[] serviceIds = packageService.Select(x => x.ServiceId).ToArray();
                            if (serviceIds != null && serviceIds.Length > 0)
                            {
                                userDto.Services = await dbContext.Service.Where(x => serviceIds.Contains(x.Id)).ToListAsync();
                            }
                        }
                    }
                }
                return userDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpGet]
        [Route("GetUsersForAttorney")]
        public async Task<IActionResult> GetUsersForAttorney(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == ParentId).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<User> users = await userService.GetUsersAsync(Status, ParentId);
                        var usersList = users.ToList().Where(x => x.ParentId == getCurrentParent.Id && x.RoleName != "Admin").ToList();
                        return Ok(usersList);
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<User> users = new List<User>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetFinancialAccounts")]
        public async Task<IActionResult> GetFinancialAccounts()
        {
            try
            {
                
                List<FinancialDetails> financialDetails = new List<FinancialDetails>();

                financialDetails = await financialService.GetFinancialAccounts();
                    
                return Ok(financialDetails);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetUsersClient")]
        public async Task<IActionResult> GetUsersClient(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<Contact> users = dbContext.Contact.ToList().Where(x=>x.FirmId==CurrentFirm.Id).ToList();
                        users[0].firm = CurrentFirm;
                        return Ok(users);
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<Contact> users = new List<Contact>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCasesClient")]
        public async Task<IActionResult> GetCasesClient(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<CaseDetail> caseDetails = dbContext.CaseDetail.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        if (caseDetails != null)
                        {
                            caseDetails[0].firm = CurrentFirm;
                            for (int i = 0; i < caseDetails.Count(); i++)
                            {
                                var practiceArea = dbContext.PracticeArea.ToList().Where(x => x.Id == caseDetails[i].PracticeArea).FirstOrDefault();
                                if (practiceArea != null)
                                {
                                caseDetails[i].PracticeAreaName = practiceArea.PracticeAreaName;
                            }
                            }
                            return Ok(caseDetails);
                        }
                        else
                        {
                            List<CaseDetail> caseDetail = new List<CaseDetail>();
                            return Ok(caseDetail);
                        }
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<Contact> users = new List<Contact>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetActiveDirectoryUsers")]
        public IActionResult GetActiveDirectoryUsers()
        {
            try
            {
                try
                {
                    //List<User> users = await userService.GetUsersAsync();
                    List<User> users = new List<User>();
                    var userName = "absol\\ADMINISTRATOR";
                    var pwd = "Server@321";
                    string path = "LDAP://173.208.142.70:6386/CN=LostAndFound,ou=App1,DC=absol,DC=com";
                    DirectoryEntry dEntry = new DirectoryEntry(path, userName, pwd);
                    DirectorySearcher dSearcher = new DirectorySearcher(dEntry);
                    dSearcher.Filter = "(&(objectClass=user)(objectCategory=user) (sAMAccountName={0})," + userName + ")";
                    dSearcher.FindAll();
                    const string SamAccountNameProperty = "sAMAccountName";
                    const string CanonicalNameProperty = "CN";
                    const string mail = "mail";
                    DirectoryEntry searchRoot2 = new DirectoryEntry(path, userName, pwd);
                    foreach (DirectoryEntry child in searchRoot2.Children)
                    {
                        // code
                    }
                    var searcher = new DirectorySearcher(searchRoot2)
                    {
                        Filter = "(&(&(objectClass=users)(objectClass=users)))"
                    };
                    var resultCollection = searcher.FindAll();
                    using (DirectoryEntry searchRoot = new DirectoryEntry(path, userName, pwd))
                    using (DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot))
                    {
                        directorySearcher.Filter = "(&(objectCategory=user)(objectClass=user))";
                        directorySearcher.PropertiesToLoad.Add(CanonicalNameProperty);
                        directorySearcher.PropertiesToLoad.Add(SamAccountNameProperty);
                        directorySearcher.PropertiesToLoad.Add(mail);
                        using (SearchResultCollection searchResultCollection = directorySearcher.FindAll())
                        {
                            foreach (SearchResult searchResult in searchResultCollection)
                            {
                                var user1 = new User();
                                if (searchResult.Properties[mail].Count > 0)
                                {
                                    user1.Email = searchResult.Properties[mail][0].ToString();
                                }
                                else
                                {
                                    user1.Email = string.Empty;
                                }
                                //if (searchResult.Properties[CanonicalNameProperty].Count > 0)
                                //{
                                //    user1.GivenName = searchResult.Properties[CanonicalNameProperty][0].ToString();
                                //}
                                //else
                                //{
                                //    user1.GivenName = string.Empty;
                                //}
                                //if (searchResult.Properties[SamAccountNameProperty].Count > 0)
                                //{
                                //    user1.Surname = searchResult.Properties[SamAccountNameProperty][0].ToString();
                                //}
                                //else
                                //{
                                //    user1.Surname = string.Empty;
                                //}
                                //if (searchResult.Properties[SamAccountNameProperty].Count > 0)
                                //{
                                //    user1.DisplayName = searchResult.Properties[SamAccountNameProperty][0].ToString();
                                //}
                                //else
                                //{
                                //    user1.DisplayName = string.Empty;
                                //}
                                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();
                                int code = (int)userEntry.Properties["userAccountControl"][0];
                                if (code == 66050)
                                {
                                    user1.LockoutEnabled = true;
                                }
                                else
                                {
                                    user1.LockoutEnabled = false;
                                }
                                users.Add(user1);
                            }
                        }
                    }
                    if (users != null && users.Count > 0)
                    {
                       // return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<User>> { Status = "Ok", Message = "RecordFound", Data = users });
                        return StatusCode(StatusCodes.Status200OK);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK);
                        // return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<User>> { Status = "Ok", Message = "RecordNotFound", Data = null });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        [Authorize(Roles = "Attorney,Customer,Admin")]
        [HttpGet]
        [Route("GetFirmUsers")]
        public async Task<IActionResult> GetFirmUsers(string Status = "", string ParentId = "")
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<User> users = await userService.GetUsersAsync(Status, getCurrentParent.Id);
                        return Ok(users.ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                else
                {
                    List<User> users = new List<User>();
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        [HttpPost]
        [Route("RegisterTestUser")]
        public async Task<IActionResult> RegisterTestUser(TestUser model)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists == null)
                {
                    await userService.AddTestAsync(model);
                    return Ok("User added Sucessfully");
                }
                else
                    return Ok("User with this email already exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
