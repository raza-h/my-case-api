using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly EmailService emailService;
        private readonly IConfiguration config;
        private readonly IActivityService activityService;
        private readonly IFinancialDetailsService financialService;
        public ContactController(IContactService contactService,IUserService userService, IMapper mapper, UserManager<User> userManager, EmailService emailService, IConfiguration config, IActivityService activityService, IFinancialDetailsService financialService, ApiDbContext dbContext)
        {
            this.contactService = contactService;
            this.userService = userService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.emailService = emailService;
            this.config = config;
            this.activityService = activityService;
            this.financialService = financialService;
            this.dbContext = dbContext;
        }
       
        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentIdCheck = loggedInUser != null ? loggedInUser.Id : null;
                var CurrentParentCheck = dbContext.User.ToList().Where(x => x.Id == ParentIdCheck).FirstOrDefault();
                if (CurrentParentCheck != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == CurrentParentCheck.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        contact.FirmId = CurrentFirm.Id;
                        contact.ParentId = CurrentParentCheck.Id;
                    }
                    else
                    {
                        return BadRequest("Please Add Firm Details First");
                    }
                }
                else
                {
                    return BadRequest("Invalid Attorney Account User");
                }

                if (contact.IsClientEnable == true)
                {
                  
                    User user = mapper.Map<User>(contact);
                    user.RoleName = "Client";
                    user.PhoneNumber = contact.CellPhone;
                    user.Password = "Client123@";
                    //user.ParentId = loggedInUser.Id;
                    user.PasswordHash = "Client123@";
                    var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                    var CurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                    if(CurrentParent != null)
                    {
                            var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == CurrentParent.Id).FirstOrDefault();
                            if (CurrentFirm != null)
                            {
                                contact.FirmId = CurrentFirm.Id;
                            }
                            else
                            {
                                return BadRequest("Please Add Firm Details First");
                            }
                        user.ParentId = CurrentParent.Id;
                        string userId = await userService.AddAsync(user);
                        contact.UserId = userId;
                      
                    }
                    else
                    {
                        return BadRequest("Invalid Attorney Account User");
                    }
                }

                string webUrl = config.GetValue<string>("webbaseurl");
                string url = $"{webUrl}Security/Account/ResetPassword?email={contact.Email}";
                bool isEmailSent = emailService.SendEmail(contact.Email, loggedInUser != null ? loggedInUser.FirstName : string.Empty, url);

                await activityService.AddActivity("Add Contact", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} invited {contact.FirstName} as contact", loggedInUser.Id);
                int Id = await contactService.AddContactAsync(contact);
                if (Id > 0)
                {
                    FinancialDetails financialDetails = new FinancialDetails
                    {
                        Name = $"{contact.FirstName} {contact.LastName}",
                        UserId = contact.UserId,
                        Type = Usertype.Client
                    };
                    await financialService.AddFinancialDetailsAsync(financialDetails);

                    for (int i = 0; i < contact.cfieldValue.Count(); i++)
                    {
                        contact.cfieldValue[i].ConcernID = Id;
                        await dbContext.CFieldValue.AddAsync(contact.cfieldValue[i]);
                        await dbContext.SaveChangesAsync();
                    }




                    if (!isEmailSent)
                    {
                        return BadRequest("ISP does not allow to email to send But Client registered Successfuly.");
                    }
                    else
                    {
                        return Ok("Contact Added successfully");
                    }
                }
                else
                    return BadRequest("something went wrong while adding contact");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetContacts")]
        public async Task<IActionResult> GetContacts(int contactGroupId = 0)
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
                        List<Contact> contacts = await contactService.GetContactsAsync(contactGroupId);
                        return Ok(contacts.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetContactsByCompanyIds")]
        public async Task<ActionResult<CaseApiResult<string>>> GetContactsByCompanyIds(string companies)
        {
            try
            {
                List<int?> companyIds = new List<int?>();
                string[] com = companies.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (!string.IsNullOrEmpty(companies))
                {
                    foreach(string companyid in com)
                    {
                        companyIds.Add(Convert.ToInt32(companyid));
                    }
                }
                List<Contact> contacts = await contactService.GetContactsByCompanyIdAsync(companyIds);
                return new CaseApiResult<string>
                {
                    Data = JsonConvert.SerializeObject(contacts),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Exception = null
                };
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
                Contact contact = await contactService.GetByIdAsync(Id);
                if (contact != null)
                    return Ok(contact);
                else
                    return NotFound("contact not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Contact contact)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                contact = await contactService.UpdateAsync(contact);
                await activityService.AddActivity("Update Contact", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as contact", loggedInUser.Id);
                return Ok(contact);
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
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await contactService.DeleteAsync(Id);
                await activityService.AddActivity("Delete Contact", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")}  deleted as contact", loggedInUser.Id);
                return Ok("contact Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
