using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MyCaseApi.Controllers
{
    //[Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class AdministratorController : CommonController
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

        public AdministratorController(IWebHostEnvironment env, UserManager<User> userManager,

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

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return Ok();
        //}

        [HttpGet]
        [Route("GetAdminUsers")]
        public IActionResult GetAdminUsers(string Status = "", string ParentId = "")
        {
            try
            {
                var getusers = dbContext.User.ToList().Where(x=>x.RoleName== "Admin").ToList();
                if (getusers != null)
                {
                    List<User> users = new List<User>(getusers);
                    return Ok(users);
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
        [Route("CheckUserExistance")]
        public IActionResult CheckUserExistance(string email)
        {
            try
            {
                var getusers = dbContext.User.ToList().Where(x=>x.Email==email).ToList();
                if (getusers.Count>0)
                {
                    return Ok("already exist");
                }
                else
                {
                    
                    return Ok("not found");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPendingUsersCustomer")]
        public IActionResult GetPendingUsersCustomer()
        {
            try
            {
                List<User> users = new List<User>();
                var getusers = dbContext.User.ToList().Where(x=>x.RoleName=="Customer").ToList();
                
                foreach (var item in getusers)
                {
                    var status =item.VerificationStatus.ToString();
                    if (status == "Pending")
                    {
                        users.Add(item);
                    }
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetApprovedUsersCustomer")]
        public IActionResult GetApprovedUsersCustomer()
        {
            try
            {
                List<User> users = new List<User>();
                var getusers = dbContext.User.ToList().Where(x=>x.RoleName=="Customer").ToList();
                
                foreach (var item in getusers)
                {
                    var status =item.VerificationStatus.ToString();
                    if (status == "Approved")
                    {
                        users.Add(item);
                    }
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetRejectedUsersCustomer")]
        public IActionResult GetRejectedUsersCustomer()
        {
            try
            {
                List<User> users = new List<User>();
                var getusers = dbContext.User.ToList().Where(x=>x.RoleName=="Customer").ToList();
                
                foreach (var item in getusers)
                {
                    var status =item.VerificationStatus.ToString();
                    if (status == "Rejected")
                    {
                        users.Add(item);
                    }
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }




        [Authorize(Roles = "Admin")]
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
                    List<User> users = await userService.GetUsersAsync(Status, ParentId);
                    return Ok(users.ToList().Where(x => x.ParentId == getCurrentParent.Id && x.RoleName == "Admin").ToList());
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
    }
}
