using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
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
    [Authorize(Roles = "Attorney,Customer,Staff,Admin,Client")]
    [Route("api/[controller]")]
    [ApiController]

    public class CaseController : ControllerBase
    {
        private readonly ICaseService caseService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        public CaseController(ICaseService caseService, IMapper mapper, ApiDbContext dbContext, IActivityService activityService, UserManager<User> userManager)
        {
            this.caseService = caseService;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.activityService = activityService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("AddCase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CaseApiResult<string>>> AddCase(CaseViewModel caseDetail)
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
                        string[] clients = caseDetail.BillingContactList.Split(',');

                        for (int i = 0; i < clients.Count(); i++)
                        {
                            if (clients[i] != "")
                            {
                                var id = Convert.ToInt64(clients[i]);

                                var getclients = dbContext.Contact.ToList().Where(x => x.ContactId == id).FirstOrDefault();
                                if (getclients != null)
                                {
                                    caseDetail.FirmId = GetFirm.Id;
                                    caseDetail.BillingContact = getclients.ContactId;
                                    CaseDetail CaseDetailedMap = mapper.Map<CaseDetail>(caseDetail);
                                    CaseDetailedMap.DateOpened= DateTime.Now;
                                    CaseDetailedMap.IsOpen= true;
                                    int Id = await caseService.AddCase(CaseDetailedMap);
                                    if (Id > 0)
                                    {
                                        foreach (var item in caseDetail.staflist)
                                        {
                                            UserAgainstCase model = new UserAgainstCase();
                                            model.CaseId = Id;
                                            model.BillingType = item.BillingRate;
                                            model.Rate = item.Rate;
                                            model.UserId = item.StafId;
                                            await dbContext.UserAgainstCase.AddAsync(model);
                                            await dbContext.SaveChangesAsync();
                                            await activityService.AddActivity("Add Case", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {caseDetail.CaseName} case", loggedInUser.Id);
                                        }
                                        for (int j = 0; j < caseDetail.cfieldValue.Count(); j++)
                                        {
                                            caseDetail.cfieldValue[j].ConcernID = Id;
                                            await dbContext.CFieldValue.AddAsync(caseDetail.cfieldValue[j]);
                                            await dbContext.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    //caseDetail.FirmId = GetFirm.Id;
                                    //CaseDetail CaseDetailedMap = mapper.Map<CaseDetail>(caseDetail);
                                    //int Id = await caseService.AddCase(CaseDetailedMap);

                                    //if (Id > 0)
                                    //{
                                    //    foreach (var item in caseDetail.staflist)
                                    //    {
                                    //        UserAgainstCase model = new UserAgainstCase();
                                    //        model.CaseId = Id;
                                    //        model.BillingType = item.BillingRate;
                                    //        model.Rate = item.Rate;
                                    //        model.UserId = item.StafId;
                                    //        await dbContext.UserAgainstCase.AddAsync(model);
                                    //        await dbContext.SaveChangesAsync();
                                    //        await activityService.AddActivity("Add Case", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {caseDetail.CaseName} case", loggedInUser.Id);
                                    //    }
                                    //}
                                }
                            }

                        }

                        return new CaseApiResult<string>
                        {
                            Data = string.Empty,
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Exception = null
                        };
                    }
                    else
                    {
                        return BadRequest("Please Add Firm Details First");
                    }
                }
                return BadRequest("Invalid Attorney Account User");


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
        [HttpGet]
        [Route("GetCases")]
        public async Task<IActionResult> GetCases()
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
                        List<CaseDetail> cases = await caseService.GetCases();
                        return Ok(cases.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetCasesCustomFields")]
        public async Task<IActionResult> GetCasesCustomFields()
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
                        List<CaseDetail> caseDetails=dbContext.CaseDetail.ToList().Where(x=>x.FirmId==CurrentFirm.Id).ToList();
                        List<CustomField> customFields = dbContext.CustomField.ToList().Where(x=>x.Type== "Cases").ToList();
                        var data = customFields.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        caseDetails.ElementAt(0).customField= data;
                        return Ok(caseDetails);
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
        [Route("GetCasesClient")]
        public async Task<IActionResult> GetCasesClient()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                if (ParentId != null)
                {

                    List<CaseDetail> cases = await caseService.GetCasesByClientId(ParentId);
                    return Ok(cases);
                }
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCasesById")]
        public async Task<IActionResult> GetCasesById(int Id)
        {
            try
            {
                CaseDetail cases = await caseService.GetCasesById(Id);
                var getName = dbContext.PracticeArea.ToList().Where(x => x.Id == cases.PracticeArea).FirstOrDefault();
                cases.PracticeAreaName = getName.PracticeAreaName;
                var getStaff = dbContext.UserAgainstCase.ToList().Where(x => x.CaseId == cases.Id).ToList();
                cases.userAgainstCase = getStaff;
                for (int i = 0; i < getStaff.Count(); i++)
                {
                    var getuser = dbContext.User.ToList().Where(x => x.Id == getStaff[i].UserId).FirstOrDefault();
                    foreach (var item in cases.userAgainstCase)
                    {
                        item.Name = getuser.FirstName + getuser.LastName;
                        item.Email= getuser.Email;
                    }
                }
                
                Contact contact = dbContext.Contact.Find(cases.BillingContact);
                cases.contact= contact;
                var customFields = dbContext.CustomField.Where(x => x.Type == "Cases").ToList();
                if (customFields!=null)
                {
                    cases.customField= customFields;
                }
                var customFieldsValue = dbContext.CFieldValue.Where(x => x.ConcernID == cases.Id).ToList();
                if (customFieldsValue != null)
                {
                    cases.cfieldValue = customFieldsValue;
                }
                var workflowData = dbContext.WorkflowAttach.Where(x => x.CaseId == cases.Id).ToList();
                if (workflowData != null)
                {
                    cases.workflowAttach = workflowData;
                    for (int i = 0; i < workflowData.Count(); i++)
                    {
                        var getworkflow = dbContext.WorkflowBase.Where(x => x.WorkflowId == workflowData[i].WorkflowId).FirstOrDefault();
                        foreach (var item in cases.workflowAttach)
                        {
                            item.WorkflowName = getworkflow.WorkflowName;
                        }
                    }
                }
                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCasesCreatedBy")]
        public async Task<IActionResult> GetCasesCreatedBy(string CreatedBy)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var currentUser = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (currentUser != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == currentUser.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<CaseDetail> cases = await caseService.GetCases();
                        List<CaseDetail> casesCreatedby = cases.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        List<CustomField> customFields = dbContext.CustomField.ToList().Where(x=>x.Type== "Cases").ToList();
                        
                        if (casesCreatedby != null && casesCreatedby.Count > 0)
                            casesCreatedby = casesCreatedby.OrderByDescending(x => x.Id).ToList();
                        var caseData = casesCreatedby.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        caseData.ElementAt(0).customField = customFields;
                        return Ok(caseData);
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
        [Route("GetCasesCreatedByTotal")]
        public async Task<IActionResult> GetCasesCreatedByTotal(string CreatedBy)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var currentUser = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (currentUser != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == currentUser.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<CaseDetail> cases = await caseService.GetCases();
                        List<CaseDetail> casesCreatedby = cases.ToList().GroupBy(x => x.CaseNumber).Select(i => i.LastOrDefault()).Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        if (casesCreatedby != null && casesCreatedby.Count > 0)
                            casesCreatedby = casesCreatedby.OrderByDescending(x => x.Id).ToList();
                        return Ok(casesCreatedby.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetCasesByClientId")]
        public async Task<IActionResult> GetCasesByClientId(string userId)
        {
            try
            {
                List<CaseDetail> cases = await caseService.GetCasesByClientId(userId);
                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCasesByStaffId")]
        public async Task<IActionResult> GetCasesByStaffId(string userId)
        {
            try
            {
                List<CaseDetail> cases = await caseService.GetCasesByStaffId(userId);
                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCasesLocation")]
        public async Task<IActionResult> GetCasesLocation(string userId, int CaseId)
        {
            try
            {
                var getLocation = await caseService.GetCasesLocation(userId, CaseId);
                if (getLocation != null)
                {
                    return Ok(getLocation);
                }
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCasesLocation")]
        public async Task<IActionResult> AddCasesLocation(ClientLocation clientLocation)
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
                        clientLocation.FirmId = GetFirm.Id;
                        clientLocation.UserId = loggedInUser.Id;
                        int Id = await caseService.AddCasesLocation(clientLocation);
                        if (Id > 0)
                        {
                            return Ok("Added Sucessfully");
                        }
                        else
                        {
                            return BadRequest("something Went Wrong");
                        }
                    }
                }
                return BadRequest("something Went Wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("CloseCase")]
        public async Task<IActionResult> CloseCase(int id)
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

                        int Id = await caseService.CloseCase(id);
                        if (Id > 0)
                        {
                            return Ok("Case Close Sucessfully");
                        }
                        else
                        {
                            return BadRequest("Something Went Wrong");
                        }
                    }
                }
                return BadRequest("Something Went Wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
