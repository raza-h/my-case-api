using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
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
    public class ConfigManagementController : ControllerBase
    {
        private readonly IConfigManagement configManagement;
        private readonly ApiDbContext dbContext;
        private readonly UserManager<User> userManager;
        public ConfigManagementController(IConfigManagement configManagement, UserManager<User> userManager, ApiDbContext dbContext)
        {
            this.configManagement = configManagement; 
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        #region contact group
        [HttpPost]
        [Route("AddContactGroup")]
        public async Task<IActionResult> AddContactGroup(ContactGroup contactGroup)
        {            
            try
            {
                int Id = await configManagement.AddContactGroupAsync(contactGroup);
                if (Id > 0)
                    return Ok("contact group added successfully");
                else
                    return BadRequest("Error while adding contact group");
            }
            catch(Exception ex)
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
                List<ContactGroup> contactGroups = await configManagement.GetContactGroupsAsync();
                return Ok(contactGroups);
            }
            catch(Exception ex)
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
                ContactGroup contactGroup = await configManagement.GetContactGroupByIdAsync(Id);
                return Ok(contactGroup);
            }
            catch(Exception ex)
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
                contactGroup = await configManagement.UpdateContactGroupAsync(contactGroup);
                return Ok(contactGroup);
            }
            catch(Exception ex)
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
                await configManagement.DeleteContactGroupAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch(Exception ex)
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
                int Id = await configManagement.AddPracticeAreaAsync(practiceArea);
                if (Id > 0)
                    return Ok("practice area added successfully");
                else
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
                List<PracticeArea> practiceAreas = await configManagement.GetPracticeAreasAsync();
                return Ok(practiceAreas);
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
                PracticeArea practiceArea = await configManagement.GetPracticeAreaByIdAsync(Id);
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
                practiceArea = await configManagement.UpdatePracticeAreaAsync(practiceArea);
                return Ok(practiceArea);
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
                await configManagement.DeletePracticeAreaAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region user title
        [HttpPost]
        [Route("AddUserTitle")]
        public async Task<IActionResult> AddUserTitle(UserTitle userTitle)
        {
            try
            {
                int Id = await configManagement.AddUserTitleAsync(userTitle);
                if (Id > 0)
                    return Ok("user title added successfully");
                else
                    return BadRequest("Error while adding user title");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserTitles")]
        public async Task<IActionResult> GetUserTitles()
        {
            try
            {
                List<UserTitle> userTitles = await configManagement.GetUserTitlesAsync();
                return Ok(userTitles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserTitleById")]
        public async Task<IActionResult> GetUserTitleById(int Id)
        {
            try
            {
                UserTitle userTitle = await configManagement.GetUserTitleByIdAsync(Id);
                return Ok(userTitle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateUserTitle")]
        public async Task<IActionResult> UpdateUserTitle(UserTitle userTitle)
        {
            try
            {
                userTitle = await configManagement.UpdateUserTitleAsync(userTitle);
                return Ok(userTitle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUserTitle")]
        public async Task<IActionResult> DeleteUserTitle(int Id)
        {
            try
            {
                await configManagement.DeleteUserTitleAsync(Id);
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
                int Id = await configManagement.AddRefferalSourceAsync(refferalSource);
                if (Id > 0)
                    return Ok("refferal source added successfully");
                else
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
                List<RefferalSource> refferalSource = await configManagement.GetRefferalSourcesAsync();
                return Ok(refferalSource);
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
                RefferalSource refferalSource = await configManagement.GetRefferalSourceByIdAsync(Id);
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
                refferalSource = await configManagement.UpdateRefferalSourceAsync(refferalSource);
                return Ok(refferalSource);
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
                await configManagement.DeleteRefferalSourceAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Billing Method
        [HttpPost]
        [Route("AddBillingMethod")]
        public async Task<IActionResult> AddBillingMethod(BillingMethod billingMethod)
        {
            try
            {
                int Id = await configManagement.AddBillingMethodAsync(billingMethod);
                if (Id > 0)
                    return Ok("billing method added successfully");
                else
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
                List<BillingMethod> billingMethods = await configManagement.GetBillingMethodsAsync();
                return Ok(billingMethods);
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
                BillingMethod billingMethod = await configManagement.GetBillingMethodByIdAsync(Id);
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
                billingMethod = await configManagement.UpdateBillingMethodAsync(billingMethod);
                return Ok(billingMethod);
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
                await configManagement.DeleteBillingMethodAsync(Id);
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
                        List<NotesTag> _resultModel = await configManagement.GetNotesTagAsync();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.NotesTagName.Equals(model.NotesTagName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Note Tag Already Exist");
                        }
                        else {
                            int Id = await configManagement.AddNotesTagAsync(model);
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
                        List<NotesTag> _resultModel = await configManagement.GetNotesTagAsync();
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
                NotesTag _notesTag = await configManagement.GetNotesTagByIdAsync(Id);
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
                model = await configManagement.UpdateNotesTagAsync(model);
                return Ok(model);
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
                await configManagement.DeleteNotesTagAsync(Id);
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
                        List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
                        bool checkname = _resultModel.ToList().Where(x => x.FirmId == GetFirm.Id).ToList().Exists(p => p.DocumentTagName.Equals(model.DocumentTagName, StringComparison.CurrentCultureIgnoreCase));
                        if (checkname)
                        {
                            return BadRequest("Document Tag Already Exist");
                        }
                        else
                        {
                            int Id = await configManagement.AddDocumentTagAsync(model);
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
                        List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
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
                DocumentTag _documentTag = await configManagement.GetDocumentTagByIdAsync(Id);
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
                model = await configManagement.UpdateDocumentTagAsync(model);
                return Ok(model);
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
                await configManagement.DeleteDocumentTagAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        [HttpGet]
        [Route("GetNotesTagClient")]
        public async Task<IActionResult> GetNotesTagClient()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
               // var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<NotesTag> _resultModel = await configManagement.GetNotesTagAsync();
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
        [Route("GetNotesTagStaff")]
        public async Task<IActionResult> GetNotesTagStaff()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
               // var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<NotesTag> _resultModel = await configManagement.GetNotesTagAsync();
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
        [Route("GetDocumentTagClient")]
        public async Task<IActionResult> GetDocumentTagClient()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
               // var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
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
        [Route("GetDocumentTagStaff")]
        public async Task<IActionResult> GetDocumentTagStaff()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
               // var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
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





        /// by asim ////////
        #region Admin Document tag
        [HttpPost]
        [Route("AddAdminDocumentTag")]
        public async Task<IActionResult> AddAdminDocumentTag(DocumentTag model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    model.UserId = getCurrentParent.Id;
                    List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
                    bool checkname = _resultModel.ToList().Where(x => x.UserId == model.UserId).ToList().Exists(p => p.DocumentTagName.Equals(model.DocumentTagName, StringComparison.CurrentCultureIgnoreCase));
                    if (checkname)
                    {
                        return BadRequest("Document Tag Already Exist");
                    }
                    else
                    {
                        int Id = await configManagement.AddDocumentTagAsync(model);
                        if (Id > 0)
                            return Ok("Document Tag added successfully");
                        else
                            return BadRequest("Error while adding document tag");
                    }


                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAdminDocumentTag")]
        public async Task<IActionResult> GetAdminDocumentTag()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    List<DocumentTag> _resultModel = await configManagement.GetDocumentTagAsync();
                    return Ok(_resultModel.ToList().Where(x => x.UserId == getCurrentParent.Id).ToList());


                }
                return BadRequest("Invalid Admin Account User");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAdminDocumentTagById")]
        public async Task<IActionResult> GetAdminDocumentTagById(int Id)
        {
            try
            {
                DocumentTag _documentTag = await configManagement.GetDocumentTagByIdAsync(Id);
                return Ok(_documentTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateAdminDocumentTag")]
        public async Task<IActionResult> UpdateAdminDocumentTag(DocumentTag model)
        {
            try
            {
                model = await configManagement.UpdateDocumentTagAsync(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAdminDocumentTag")]
        public async Task<IActionResult> DeleteAdminDocumentTag(int Id)
        {
            try
            {
                await configManagement.DeleteDocumentTagAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}