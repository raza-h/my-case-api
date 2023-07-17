using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using MyCaseApi.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Staff,Admin,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttorneyAdminController : ControllerBase
    {
        private readonly IAttorneyAdmin attorneyAdmin;
        private readonly ApiDbContext dbContext;
        private readonly UserManager<User> userManager;
        public AttorneyAdminController(IAttorneyAdmin attorneyAdmin, UserManager<User> userManager, ApiDbContext dbContext)
        {
            this.attorneyAdmin = attorneyAdmin;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }



        #region Billing Method
        [HttpPost]
        [Route("AddBillingMethod")]
        public async Task<IActionResult> AddBillingMethod(BillingMethod billingMethod)
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
                        billingMethod.FirmId = GetFirm.Id;
                        billingMethod.UserId = getCurrentParent.Id;
                        int Id = await attorneyAdmin.AddBillingMethodAsync(billingMethod);
                        if (Id > 0)
                            return Ok("billing method added successfully");
                        else
                            return BadRequest("Error while adding billing method");
                    }
                    else
                    {
                        return BadRequest("Please Add Firm Details First");
                    }
                }
                return BadRequest("Error while adding billing method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBillingMethods")]
        public async Task<IActionResult> GetBillingMethods()
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
                        List<BillingMethod> billingMethods = await attorneyAdmin.GetBillingMethodsAsync();
                        return Ok(billingMethods.ToList().Where(x => x.FirmId == GetFirm.Id).ToList());
                    }
                }
                return BadRequest("Error while getting billing method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBillingMethodById")]
        public async Task<IActionResult> GetBillingMethodById(int Id)
        {
            try
            {
                BillingMethod billingMethod = await attorneyAdmin.GetBillingMethodByIdAsync(Id);
                return Ok(billingMethod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateBillingMethod")]
        public async Task<IActionResult> UpdateBillingMethod(BillingMethod billingMethod)
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
                        billingMethod.FirmId = GetFirm.Id;
                        billingMethod.UserId = getCurrentParent.Id;
                        billingMethod = await attorneyAdmin.UpdateBillingMethodAsync(billingMethod);
                        return Ok(billingMethod);
                    }
                }
                return BadRequest("Error while Updating billing method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteBillingMethod")]
        public async Task<IActionResult> DeleteBillingMethod(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteBillingMethodAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region refferal source
        [HttpPost]
        [Route("AddRefferalSource")]
        public async Task<IActionResult> AddRefferalSource(RefferalSource refferalSource)
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
                        refferalSource.FirmId = GetFirm.Id;
                        refferalSource.UserId = getCurrentParent.Id;
                        int Id = await attorneyAdmin.AddRefferalSourceAsync(refferalSource);
                        if (Id > 0)
                            return Ok("refferal source added successfully");
                        else
                            return BadRequest("Error while adding refferal source");
                    }
                }
                return BadRequest("Error while adding refferal source");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRefferalSources")]
        public async Task<IActionResult> GetRefferalSources()
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
                        List<RefferalSource> refferalSource = await attorneyAdmin.GetRefferalSourcesAsync();
                        return Ok(refferalSource.ToList().Where(x => x.FirmId == GetFirm.Id).ToList());
                    }
                }
                return BadRequest("Error while adding billing method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRefferalSourceById")]
        public async Task<IActionResult> GetRefferalSourceById(int Id)
        {
            try
            {
                RefferalSource refferalSource = await attorneyAdmin.GetRefferalSourceByIdAsync(Id);
                return Ok(refferalSource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateRefferalSource")]
        public async Task<IActionResult> UpdateRefferalSource(RefferalSource refferalSource)
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
                        refferalSource.FirmId = GetFirm.Id;
                        refferalSource.UserId = getCurrentParent.Id;
                        refferalSource = await attorneyAdmin.UpdateRefferalSourceAsync(refferalSource);
                        return Ok(refferalSource);
                    }
                }
                return BadRequest("Error while updating refferal source");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteRefferalSource")]
        public async Task<IActionResult> DeleteRefferalSource(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteRefferalSourceAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region practice area
        [HttpPost]
        [Route("AddPracticeArea")]
        public async Task<IActionResult> AddPracticeArea(PracticeArea practiceArea)
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
                        practiceArea.FirmId = GetFirm.Id;
                        practiceArea.UserId = getCurrentParent.Id;
                        int Id = await attorneyAdmin.AddPracticeAreaAsync(practiceArea);
                        if (Id > 0)
                            return Ok("practice area added successfully");
                        else
                            return BadRequest("Error while adding practice area");
                    }
                }
                return BadRequest("Error while adding practice area");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPracticeAreas")]
        public async Task<IActionResult> GetPracticeAreas()
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
                        List<PracticeArea> practiceAreas = await attorneyAdmin.GetPracticeAreasAsync();
                        return Ok(practiceAreas.ToList().Where(x => x.FirmId == GetFirm.Id).ToList());
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPracticeAreaById")]
        public async Task<IActionResult> GetPracticeAreaById(int Id)
        {
            try
            {
                PracticeArea practiceArea = await attorneyAdmin.GetPracticeAreaByIdAsync(Id);
                return Ok(practiceArea);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdatePracticeArea")]
        public async Task<IActionResult> UpdatePracticeArea(PracticeArea practiceArea)
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
                        practiceArea.FirmId = GetFirm.Id;
                        practiceArea.UserId = getCurrentParent.Id;
                        practiceArea = await attorneyAdmin.UpdatePracticeAreaAsync(practiceArea);
                        return Ok(practiceArea);
                    }
                }
                return BadRequest("Error while updating practice area");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePracticeArea")]
        public async Task<IActionResult> DeletePracticeArea(int Id)
        {
            try
            {
                await attorneyAdmin.DeletePracticeAreaAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region contact group
        [HttpPost]
        [Route("AddContactGroup")]
        public async Task<IActionResult> AddContactGroup(ContactGroup contactGroup)
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
                        contactGroup.FirmId = GetFirm.Id;
                        contactGroup.UserId = getCurrentParent.Id;
                        int Id = await attorneyAdmin.AddContactGroupAsync(contactGroup);
                        if (Id > 0)
                            return Ok("contact group added successfully");
                        else
                            return BadRequest("Error while adding contact group");
                    }
                }
                return BadRequest("Error while adding contact group");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetContactGroups")]
        public async Task<IActionResult> GetContactGroups()
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
                        List<ContactGroup> contactGroups = await attorneyAdmin.GetContactGroupsAsync();
                        return Ok(contactGroups.ToList().Where(x => x.FirmId == GetFirm.Id).ToList());
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetContactGroupById")]
        public async Task<IActionResult> GetContactGroupById(int Id)
        {
            try
            {
                ContactGroup contactGroup = await attorneyAdmin.GetContactGroupByIdAsync(Id);
                return Ok(contactGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateContactGroup")]
        public async Task<IActionResult> UpdateContactGroup(ContactGroup contactGroup)
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
                        contactGroup.FirmId = GetFirm.Id;
                        contactGroup.UserId = getCurrentParent.Id;
                        contactGroup = await attorneyAdmin.UpdateContactGroupAsync(contactGroup);
                        return Ok(contactGroup);
                    }
                }
                return BadRequest("Error while Updating billing method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteContactGroup")]
        public async Task<IActionResult> DeleteContactGroup(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteContactGroupAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Notes Tag

        [HttpPost]
        [Route("AddNotesTag")]
        public async Task<IActionResult> AddNotesTag(NotesTag model)
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
                        model.FirmId = GetFirm.Id;
                        List<NotesTag> _resultModel = await attorneyAdmin.GetNotesTagAsync();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.NotesTagName.Equals(model.NotesTagName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Note Tag Already Exist");
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            int Id = await attorneyAdmin.AddNotesTagAsync(model);
                            if (Id > 0)
                                return Ok("Notes Tag added successfully");
                            else
                                return BadRequest("Error while adding Note Tag");
                        }
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
        [Route("GetNotesTag")]
        public async Task<IActionResult> GetNotesTag()
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
                        List<NotesTag> _resultModel = await attorneyAdmin.GetNotesTagAsync();
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

        [HttpGet]
        [Route("GetNotesTagById")]
        public async Task<IActionResult> GetNotesTagById(int Id)
        {
            try
            {
                NotesTag _notesTag = await attorneyAdmin.GetNotesTagByIdAsync(Id);
                return Ok(_notesTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateNotesTag")]
        public async Task<IActionResult> UpdateNotesTag(NotesTag model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateNotesTagAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteNotesTag")]
        public async Task<IActionResult> DeleteNotesTag(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteNotesTagAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Document Tag

        [HttpPost]
        [Route("AddDocumentTag")]
        public async Task<IActionResult> AddDocumentTag(DocumentTag model)
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
                        model.FirmId = GetFirm.Id;
                        List<DocumentTag> _resultModel = await attorneyAdmin.GetDocumentTagAsync();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.DocumentTagName.Equals(model.DocumentTagName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Document Tag Already Exist");
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            int Id = await attorneyAdmin.AddDocumentTagAsync(model);
                            if (Id > 0)
                                return Ok("Document Tag added successfully");
                            else
                                return BadRequest("Error while adding document tag");
                        }
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
        [Route("GetDocumentTag")]
        public async Task<IActionResult> GetDocumentTag()
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
                        List<DocumentTag> _resultModel = await attorneyAdmin.GetDocumentTagAsync();
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

        [HttpGet]
        [Route("GetDocumentTagById")]
        public async Task<IActionResult> GetDocumentTagById(int Id)
        {
            try
            {
                DocumentTag _documentTag = await attorneyAdmin.GetDocumentTagByIdAsync(Id);
                return Ok(_documentTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateDocumentTag")]
        public async Task<IActionResult> UpdateDocumentTag(DocumentTag model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateDocumentTagAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteDocumentTag")]
        public async Task<IActionResult> DeleteDocumentTag(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteDocumentTagAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Did Not Hire

        [HttpPost]
        [Route("AddReason")]
        public async Task<IActionResult> AddReason(HireReason model)
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
                        model.FirmId = GetFirm.Id;
                        List<HireReason> _resultModel = await attorneyAdmin.GetReasons();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.ReasonName.Equals(model.ReasonName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Reason Already Exist");
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            int Id = await attorneyAdmin.AddReasonAsync(model);
                            if (Id > 0)
                                return Ok("Reason added successfully");
                            else
                                return BadRequest("Error while adding Reason");
                        }
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
        [Route("GetReasons")]
        public async Task<IActionResult> GetReasons()
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
                        List<HireReason> _resultModel = await attorneyAdmin.GetReasons();
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

        [HttpGet]
        [Route("GetReasonsById")]
        public async Task<IActionResult> GetReasonsById(int Id)
        {
            try
            {
                HireReason hireReason = await attorneyAdmin.GetReasonsByIdAsync(Id);
                return Ok(hireReason);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateRejectReason")]
        public async Task<IActionResult> UpdateRejectReason(HireReason model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateReasonAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteRejectReason")]
        public async Task<IActionResult> DeleteRejectReason(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteReasonAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Lead Status

        [HttpPost]
        [Route("AddLeadStatus")]
        public async Task<IActionResult> AddLeadStatus(LeadStatus model)
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
                        model.FirmId = GetFirm.Id;
                        List<LeadStatus> _resultModel = await attorneyAdmin.GetLeadStatus();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.LStatusName.Equals(model.LStatusName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Lead Status Already Exist");
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            int Id = await attorneyAdmin.AddLeadStatusAsync(model);
                            if (Id > 0)
                                return Ok("Lead Status added successfully");
                            else
                                return BadRequest("Error while adding Lead Status");
                        }
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
        [Route("GetLeadStatus")]
        public async Task<IActionResult> GetLeadStatus()
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
                        List<LeadStatus> _resultModel = await attorneyAdmin.GetLeadStatus();
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

        [HttpGet]
        [Route("GetLeadStatusId")]
        public async Task<IActionResult> GetLeadStatusId(int Id)
        {
            try
            {
                LeadStatus leadStatus = await attorneyAdmin.GetLeadStatusByIdAsync(Id);
                return Ok(leadStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateLeadStatus")]
        public async Task<IActionResult> UpdateLeadStatus(LeadStatus model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateLeadStatusAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteLeadStatus")]
        public async Task<IActionResult> DeleteLeadStatus(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteLeadStatusAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Custom Fields

        [HttpPost]
        [Route("AddCustomField")]
        public async Task<IActionResult> AddCustomField(CustomField model)
        {
            try
            {
                model.CustomFieldNametemp = model.CustomFieldName.ToLower();
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {
                        model.FirmId = GetFirm.Id;
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.CustomFieldName.Equals(model.CustomFieldName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            var getData = dbContext.CustomField.Where(x => x.FirmId == GetFirm.Id).ToList();
                            foreach (var item in getData)
                            {
                                if (item.CustomFieldName == model.CustomFieldNametemp)
                                {
                                    if (item.Type == model.Type)
                                    {
                                        return BadRequest("Custom Fields Name Already Exist");
                                    }
                                    else
                                    {
                                        model.FirmId = GetFirm.Id;
                                        model.UserId = getCurrentParent.Id;
                                        if (model.FullPractice == true)
                                        {
                                            model.FullPractice = true;
                                            model.PartialPractice = false;
                                        }
                                        else
                                        {
                                            model.FullPractice = false;
                                            model.PartialPractice = true;
                                        }
                                        int Id = await attorneyAdmin.AddCustomFieldAsync(model);
                                        if (Id > 0)
                                            return Ok("Custom Field added successfully");
                                        else
                                            return BadRequest("Error while adding Custom Field");
                                    }
                                }
                            }

                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            if (model.FullPractice == true)
                            {
                                model.FullPractice = true;
                                model.PartialPractice = false;
                            }
                            else
                            {
                                model.FullPractice = false;
                                model.PartialPractice = true;
                            }
                            int Id = await attorneyAdmin.AddCustomFieldAsync(model);
                            if (Id > 0)
                                return Ok("Custom Field added successfully");
                            else
                                return BadRequest("Error while adding Custom Field");
                        }
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
        [Route("GetCustomFields")]
        public async Task<IActionResult> GetCustomFields()
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
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
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
        [HttpGet]
        [Route("GetCaseFields")]
        public async Task<IActionResult> GetCaseFields()
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
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
                        var datalist = _resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        return Ok(datalist.ToList().Where(x => x.Type == "Cases").ToList());
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
        [Route("GetContactsFields")]
        public async Task<IActionResult> GetContactsFields()
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
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
                        var datalist = _resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        return Ok(datalist.ToList().Where(x => x.Type == "Contacts").ToList());
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
        [Route("GetCompaniesFields")]
        public async Task<IActionResult> GetCompaniesFields()
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
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
                        var datalist = _resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        return Ok(datalist.ToList().Where(x => x.Type == "Companies").ToList());
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
        [Route("GetTimeEntryFields")]
        public async Task<IActionResult> GetTimeEntryFields()
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
                        List<CustomField> _resultModel = await attorneyAdmin.GetCustomField();
                        var datalist = _resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        return Ok(datalist.ToList().Where(x => x.Type == "TimeEntry").ToList());
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
        [Route("GetCustomFieldbyId")]
        public async Task<IActionResult> GetCustomFieldbyId(int Id)
        {
            try
            {
                CustomField customField = await attorneyAdmin.GetCustomFieldByIdAsync(Id);
                return Ok(customField);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateCustomField")]
        public async Task<IActionResult> UpdateCustomField(CustomField model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateCustomFieldAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteCustomField")]
        public async Task<IActionResult> DeleteCustomField(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteCustomFieldAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCustomPracticebyId")]
        public async Task<IActionResult> GetCustomPracticebyId(int Id)
        {
            try
            {
                var customPractice = await attorneyAdmin.GetCustomPracticeByIdAsync(Id);
                return Ok(customPractice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AddCustomValue")]
        public async Task<IActionResult> AddCustomValue(List<CFieldValue> model)
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
                        for (int i = 0; i < model.Count(); i++)
                        {
                            if (model[i].ModuleType == "Cases")
                            {

                            }
                            else if (model[i].ModuleType == "Contacts")
                            {

                            }
                            else if (model[i].ModuleType == "Companies")
                            {

                            }
                            else if (model[i].ModuleType == "Expense")
                            {

                            }
                            int Id = await attorneyAdmin.AddCustomValueAsync(model[i]);

                        }

                        //if (Id > 0)
                        return Ok("Custom Value added successfully");
                        //else
                        //    return BadRequest("Error while adding Custom Field Value");
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");
            }
            catch (Exception ex)
            {
                return BadRequest("Error while adding Custom Field Value");
            }
        }
        #endregion

        #region TimeEntry

        [HttpGet]
        [Route("GetTimeEntryActivity")]
        public async Task<IActionResult> GetTimeEntryActivity()
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
                        List<TimeEntryActivity> data = await attorneyAdmin.GetTimeEntryActivity();
                        return Ok(data.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                }
                return BadRequest("Please Add Firm Details First");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AddTimeEntryActivity")]
        public async Task<IActionResult> AddTimeEntryActivity(TimeEntryActivity model)
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
                        model.FirmId = GetFirm.Id;
                        List<TimeEntryActivity> _resultModel = await attorneyAdmin.GetTimeEntryActivity();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.ActivityName.Equals(model.ActivityName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Activity Name Already Exist");
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            model.UserId = getCurrentParent.Id;
                            int Id = await attorneyAdmin.AddTimeEntryActivityAsync(model);
                            if (Id > 0)
                                return Ok("Activity Added successfully");
                            else
                                return BadRequest("Error while adding Activity");
                        }
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
        [HttpDelete]
        [Route("DeleteTimeEntryActivity")]
        public async Task<IActionResult> DeleteTimeEntryActivity(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteTimeEntryActivityAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateTimeEntryActivity")]
        public async Task<IActionResult> UpdateTimeEntryActivity(TimeEntryActivity model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model = await attorneyAdmin.UpdateTimeEntryActivityAsync(model);
                        return Ok(model);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTimeEntryActivityId")]
        public async Task<IActionResult> GetTimeEntryActivityId(int Id)
        {
            try
            {
                var data = await attorneyAdmin.GetTimeEntryActivityByIdAsync(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddTimeEntry")]
        public async Task<IActionResult> AddTimeEntry(TimeEntry model)
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
                        model.FirmId = GetFirm.Id;
                        model.UserId = getCurrentParent.Id;
                        model.AddedBy = loggedInUser.FirstName + loggedInUser.LastName;
                        if (model.RateType == "/Hr")
                        {
                            var rate = model.Rate / 60;
                            model.Total = rate * model.Duration;
                            model.Total = Math.Round(Convert.ToDouble(model.Total), 2);
                        }
                        else if (model.RateType == "FlatRate")
                        {
                            model.Total = model.Rate;
                            model.Total = Math.Round(Convert.ToDouble(model.Total), 2);
                        }
                        int Id = await attorneyAdmin.AddTimeEntryAsync(model);
                        if (Id > 0)
                        {
                            for (int j = 0; j < model.cfieldValue.Count(); j++)
                            {
                                model.cfieldValue[j].ConcernID = Id;
                                await dbContext.CFieldValue.AddAsync(model.cfieldValue[j]);
                                await dbContext.SaveChangesAsync();
                            }
                            return Ok("Time Entry Added Successfully");
                        }
                        else
                            return BadRequest("Error While Adding Time Entry");
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
        [Route("GetTimeEntry")]
        public async Task<IActionResult> GetTimeEntry()
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
                        List<TimeEntry> data = await attorneyAdmin.GetTimeEntry();
                        var firmData = data.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        var customFields = dbContext.CustomField.Where(x => x.Type == "TimeEntry").ToList();
                        if (customFields != null)
                        {
                            firmData.ElementAt(0).customField = customFields;
                        }
                        var customFieldsValue = dbContext.CFieldValue.Where(x => x.ConcernID == firmData.ElementAt(0).TimeEntryID).ToList();
                        if (customFieldsValue != null)
                        {
                            firmData.ElementAt(0).cfieldValue = customFieldsValue;
                        }
                        return Ok(firmData);
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
        [Route("GetTimeEntryById")]
        public async Task<IActionResult> GetTimeEntryById(int Id)
        {
            try
            {
                var data = await attorneyAdmin.GetTimeEntryByIdAsync(Id);
                var customFields = dbContext.CustomField.Where(x => x.Type == "TimeEntry").ToList();
                if (customFields != null)
                {
                    data.customField = customFields;
                }
                var customFieldsValue = dbContext.CFieldValue.Where(x => x.ConcernID == data.TimeEntryID).ToList();
                if (customFieldsValue != null)
                {
                    data.cfieldValue = customFieldsValue;
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteTimeEntry")]
        public async Task<IActionResult> DeleteTimeEntry(int Id)
        {
            try
            {
                await attorneyAdmin.DeleteTimeEntryAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        #endregion
    }
}
