using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService notesService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        private readonly UserManager<User> userManager;

        public NotesController(INotesService notesService, IMapper mapper, ApiDbContext dbContext, UserManager<User> userManager)
        {
            this.notesService = notesService;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("AddNotes")]
        public async Task<ActionResult<NotesApiResult<string>>> AddNotes(Notes model)
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
                        if (model.SelectBit == true)
                        {
                            var getCases = dbContext.CaseDetail.ToList().Where(x => x.FirmId == GetFirm.Id).ToList();
                            Notes notes = new Notes();
                            for (int i = 0; i < getCases.Count(); i++)
                            {
                                notes.FirmId = GetFirm.Id;
                                notes.CaseId = Convert.ToString(getCases[i].Id);
                                notes.NotesDescripation = model.NotesDescripation;
                                notes.NotesTittle = model.NotesTittle;
                                notes.NotesTag = model.NotesTag;
                                notes.NotesType = model.NotesType;
                                notes.UserId = model.UserId;
                                notes.Id = 0;
                                await notesService.AddNotesMultiple(notes);
                            }
                        }
                        else
                        {
                            model.FirmId = GetFirm.Id;
                            int Id = await notesService.AddNotes(model);
                        }

                        return new NotesApiResult<string>
                        {
                            Data = string.Empty,
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Exception = null
                        };
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
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
        [Route("GetNotes")]
        public async Task<IActionResult> GetNotes(string UserId)
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
                        List<Notes> notes = await notesService.GetNotes();
                        List<Notes> _resultModel = notes.ToList().Where(x => x.UserId == UserId).ToList();
                        var list = _resultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        for (int i = 0; i < list.Count(); i++)
                        {
                            var getCase = dbContext.CaseDetail.ToList().Where(x => x.Id == Convert.ToInt64(list[i].CaseId)).FirstOrDefault();
                            if (getCase != null)
                            {
                                list[i].CaseName = getCase.CaseName;
                            }
                            else
                            {
                                list[i].CaseName = "Personal";
                            }
                            
                        }
                        return Ok(list);
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
        [Route("GetNotesById")]
        public async Task<IActionResult> GetNotesById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Notes model = await notesService.GetNotesByid(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateNotes")]
        public async Task<IActionResult> UpdateNotes(Notes model)
        {
            try
            {
                model = await notesService.UpdateNotes(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteNotes")]
        public async Task<IActionResult> DeleteNotes(int Id)
        {
            try
            {
                await notesService.DeleteNotes(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetNotesClient")]
        public async Task<IActionResult> GetNotesClient(string UserId)
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
                        List<Notes> notes = await notesService.GetNotes();
                        List<Notes> _resultModel = notes.ToList().Where(x => x.UserId == UserId).ToList();
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

        [HttpPost]
        [Route("AddNotesClient")]
        public async Task<ActionResult<NotesApiResult<string>>> AddNotesClient(Notes model)
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
                        int Id = await notesService.AddNotes(model);
                        return new NotesApiResult<string>
                        {
                            Data = string.Empty,
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Exception = null
                        };
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
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
        [Route("GetNotesStaff")]
        public async Task<IActionResult> GetNotesStaff(string UserId)
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
                        List<Notes> notes = await notesService.GetNotes();
                        List<Notes> _resultModel = notes.ToList().Where(x => x.UserId == UserId).ToList();
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

        [HttpPost]
        [Route("AddNotesStaff")]
        public async Task<ActionResult<NotesApiResult<string>>> AddNotesStaff(Notes model)
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
                        int Id = await notesService.AddNotes(model);
                        return new NotesApiResult<string>
                        {
                            Data = string.Empty,
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Exception = null
                        };
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
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



        ////// by asim //////
        ///




        [HttpGet]
        [Route("GetAdminNotesTag")]
        public async Task<IActionResult> GetAdminNotesTag(string userId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    List<NotesTag> notesTags = await notesService.GetNotesTagAsync();
                    List<NotesTag> _resultModel = notesTags.ToList().Where(x => x.UserId == getCurrentParent.Id).ToList();
                    return Ok(_resultModel.ToList());


                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAdminNotesTag")]
        public async Task<ActionResult<NotesApiResult<string>>> AddAdminNotesTag(NotesTag model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    model.UserId = getCurrentParent.Id;
                    int Id = await notesService.AddadminNotesTagAsync(model);
                    return new NotesApiResult<string>
                    {
                        Data = string.Empty,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Exception = null
                    };
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
        [Route("GetAdminNotesTagById")]
        public async Task<IActionResult> GetAdminNotesTagById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                NotesTag model = await notesService.GetNotesTagByIdAsync(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateAdminNotesTag")]
        public async Task<IActionResult> UpdateAdminNotesTag(NotesTag model)
        {
            try
            {
                var recoreds = await notesService.UpdateNotesTagAsync(model);
                return Ok(recoreds);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAdminNotesTag")]
        public async Task<IActionResult> DeleteAdminNotesTag(int Id)
        {
            try
            {
                await notesService.DeleteNotesTagAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        [Route("AddAdminNotesDetails")]
        public async Task<ActionResult<NotesApiResult<string>>> AddAdminNotesDetails(Notes model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    model.UserId = getCurrentParent.Id;
                    int Id = await notesService.AddNotesDetails(model);
                    return new NotesApiResult<string>
                    {
                        Data = string.Empty,
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Exception = null
                    };


                }
                return BadRequest("Invalid Admin Account User");

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
        [Route("GetAdminNotesDetails")]
        public async Task<IActionResult> GetAdminNotesDetails(string UserId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    List<Notes> notes = await notesService.GetNotesDetails();
                    List<Notes> _resultModel = notes.ToList().Where(x => x.UserId == getCurrentParent.Id).ToList();
                    return Ok(_resultModel.ToList());


                }
                return BadRequest("Invalid Admin Account User");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAdminNotesDetailsById")]
        public async Task<IActionResult> GetAdminNotesDetailsById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Notes model = await notesService.GetNotesDetailsByid(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateAdminNotesDetails")]
        public async Task<IActionResult> UpdateAdminNotesDetails(Notes model)
        {
            try
            {
                var recoreds = await notesService.UpdateNotesDetails(model);
                return Ok(recoreds);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteAdminNotesDetails")]
        public async Task<IActionResult> DeleteAdminNotesDetails(int Id)
        {
            try
            {
                await notesService.DeleteNotes(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetNotesByIdforCase")]
        public async Task<IActionResult> GetNotesByIdforCase(int? Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Notes model = await notesService.GetNotesByid(_Id);

                var getcases = dbContext.CaseDetail.ToList().Where(x => x.Id != Convert.ToInt64(model.CaseId) && x.FirmId == model.FirmId).ToList();

                return Ok(getcases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("LinkNewCasetoNotes")]
        public async Task<ActionResult<NotesApiResult<string>>> LinkNewCasetoNotes(Notes model)
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
                        Notes notes = new Notes();
                        var getNotes = dbContext.Notes.ToList().Where(x =>x.Id == model.OldCaseId).FirstOrDefault();
                        if (getNotes != null)
                        {
                            model.FirmId = GetFirm.Id;
                            model.CaseId = model.CaseId;
                            model.NotesDescripation = getNotes.NotesDescripation;
                            model.NotesTittle = getNotes.NotesTittle;
                            model.NotesTag = getNotes.NotesTag;
                            model.NotesType = getNotes.NotesType;
                            model.UserId = getNotes.UserId;
                            await notesService.AddNotes(model);
                        }
                        return new NotesApiResult<string>
                        {
                            Data = string.Empty,
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                            Exception = null
                        };
                    }
                    else
                        return BadRequest("Please Add Firm Details First");
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
    }
}
