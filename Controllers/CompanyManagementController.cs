using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
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
    public class CompanyManagementController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly ApiDbContext dbContext;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        public CompanyManagementController(ICompanyService companyService, ApiDbContext dbContext, IMapper mapper, IActivityService activityService, UserManager<User> userManager)
        {
            this.companyService = companyService;
            this.dbContext = dbContext;
            this.activityService = activityService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Company company)
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
                        company.FirmId = GetFirm.Id;
                        var existingCompany = await dbContext.Company.Where(x => x.Name == company.Name).FirstOrDefaultAsync();
                        await activityService.AddActivity("Add CompanyManagement", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {company.Name} company", loggedInUser.Id);
                        if (existingCompany == null)
                        {
                            int Id = await companyService.AddCompanyAsync(company);
                            if (Id > 0)
                            {
                                for (int i = 0; i < company.cfieldValue.Count(); i++)
                                {
                                    company.cfieldValue[i].ConcernID = Id;
                                    await dbContext.CFieldValue.AddAsync(company.cfieldValue[i]);
                                    await dbContext.SaveChangesAsync();
                                }
                                return Ok("Company Added Sucessfully");
                            }
                            else
                            {
                                return BadRequest("Error Occurred while adding company");
                            }
                        }
                        else
                            return BadRequest("company with given name already exist");
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
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
                        List<Company> companies = await companyService.GetAsync();
                        return Ok(companies.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                }
                return BadRequest("Please Add Firm Details First");
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
                Company company = await companyService.GetByIdAsync(Id);
                if (company != null)
                    return Ok(company);
                else
                    return NotFound("company not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Company company)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                company = await companyService.UpdateAsync(company);
                await activityService.AddActivity("Update CompanyManagement", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated {company.Name} company", loggedInUser.Id);
                return Ok(company);
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
                await companyService.DeleteAsync(Id);
                await activityService.AddActivity("Delete CompanyManagement", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted company", loggedInUser.Id);
                return Ok("Company Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
