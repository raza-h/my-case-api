using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService leadService;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        private readonly ApiDbContext dbContext;
        public LeadController(ILeadService leadService, IActivityService activityService, UserManager<User> userManager, ApiDbContext dbContext)
        {
            this.leadService = leadService;
            this.activityService = activityService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddLead")]
        public async Task<IActionResult> AddLead(Lead lead)
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
                        lead.FirmId = GetFirm.Id;

                        var getStatus=dbContext.LeadStatus.ToList().Where(x=>x.LStatusId==lead.LStatusId).FirstOrDefault();
                        if (getStatus != null)
                        {
                            lead.LStatusId = getStatus.LStatusId;
                            lead.LStatusName= getStatus.LStatusName;
                        }
                        await activityService.AddActivity("Add Lead", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {lead.FirstName} as lead", loggedInUser.Id);
                        int Id = await leadService.AddLeadAsync(lead);
                        if (Id > 0)
                            return Ok("lead added successfully");
                        else
                            return BadRequest("Error while adding lead");
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
        [Route("GetLeads")]
        public async Task<IActionResult> GetLeads()
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
                        List<Lead> lead = await leadService.GetLeadsAsync();
                        return Ok(lead.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetLeadById")]
        public async Task<IActionResult> GetLeadById(int Id)
        {
            try
            {
                Lead lead = await leadService.GetLeadByIdAsync(Id);
                return Ok(lead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateLead")]
        public async Task<IActionResult> UpdateLead(Lead getdata)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var lead = dbContext.Lead.ToList().Where(x => x.Id == getdata.Id).FirstOrDefault();
                if (getdata.AssignTo!="" && getdata.AssignTo!="0")
                {
                    lead.AssignTo = getdata.AssignTo;
                }
                if (getdata.ContactId!=null && getdata.ContactId!=0)
                {
                    lead.ContactId = getdata.ContactId;
                }
                if (getdata.Address!=null && getdata.Address!="")
                {
                    lead.Address = getdata.Address;
                }
                if (getdata.Address2!=null && getdata.Address2 != "")
                {
                    lead.Address2 = getdata.Address2;
                }
                if (getdata.CellPhone!=null && getdata.CellPhone != "")
                {
                    lead.CellPhone = getdata.CellPhone;
                }
                if (getdata.CompanyId!=null)
                {
                    lead.CompanyId = getdata.CompanyId;
                }
                if (getdata.ContactId!=null)
                {
                    lead.ContactId = getdata.ContactId;
                }
                if (getdata.City!=null && getdata.City != "")
                {
                    lead.City = getdata.City;
                }
                if (getdata.ConflictCheck!=null)
                {
                    lead.ConflictCheck = getdata.ConflictCheck;
                }
                if (getdata.ConflictCheckNotes != null && getdata.ConflictCheckNotes != "")
                {
                    lead.ConflictCheckNotes = getdata.ConflictCheckNotes;
                }
                if (getdata.Country != null)
                {
                    lead.Country = getdata.Country;
                }
                if (getdata.DOB != null)
                {
                    lead.DOB = getdata.DOB;
                }
                if (getdata.DateAdded != null)
                {
                    lead.DateAdded = getdata.DateAdded;
                }
                if (getdata.DriverLicence != null && getdata.DriverLicence != "")
                {
                    lead.DriverLicence = getdata.DriverLicence;
                }
                if (getdata.DriverLicenceState != null && getdata.DriverLicenceState != "")
                {
                    lead.DriverLicenceState = getdata.DriverLicenceState;
                }
                if (getdata.Email != null && getdata.Email != "")
                {
                    lead.Email = getdata.Email;
                }
                if (getdata.FirmId != null)
                {
                    lead.FirmId = getdata.FirmId;
                }
                if (getdata.FirstName != null && getdata.FirstName != "")
                {
                    lead.FirstName = getdata.FirstName;
                }
                if (getdata.HomePhone != null && getdata.HomePhone != "")
                {
                    lead.HomePhone = getdata.HomePhone;
                }
                if (getdata.LastName != null && getdata.LastName != "")
                {
                    lead.LastName = getdata.LastName;
                }
                if (getdata.LeadDetails != null && getdata.LeadDetails != "")
                {
                    lead.LeadDetails = getdata.LeadDetails;
                }
                if (getdata.MidName != null && getdata.MidName != "")
                {
                    lead.MidName = getdata.MidName;
                }
                if (getdata.Office != null)
                {
                    lead.Office = getdata.Office;
                }
                if (getdata.PotentailCaseDescription != null && getdata.PotentailCaseDescription != "")
                {
                    lead.PotentailCaseDescription = getdata.PotentailCaseDescription;
                }
                if (getdata.PotentialValueCase != null)
                {
                    lead.PotentialValueCase = getdata.PotentialValueCase;
                }
                if (getdata.PracticeAreaId != null)
                {
                    lead.PracticeAreaId = getdata.PracticeAreaId;
                }
                if (getdata.RefferelSource != null)
                {
                    lead.RefferelSource = getdata.RefferelSource;
                }
                if (getdata.State != null && getdata.State != "")
                {
                    lead.State = getdata.State;
                }
                if (getdata.Status != null)
                {
                    lead.Status = getdata.Status;
                }
                if (getdata.WorkPhone != null && getdata.WorkPhone != "")
                {
                    lead.WorkPhone = getdata.WorkPhone;
                }
                if (getdata.ZipCode != null)
                {
                    lead.ZipCode = getdata.ZipCode;
                }

                await activityService.AddActivity("Update Lead", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as lead", loggedInUser.Id);
                lead = await leadService.UpdateLeadAsync(lead);
                return Ok(lead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteLead")]
        public async Task<IActionResult> DeleteLead(int Id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await activityService.AddActivity("Delete Lead", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted as lead", loggedInUser.Id);
                await leadService.DeleteLeadAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
