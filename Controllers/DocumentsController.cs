using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class DocumentsController : CommonController
    {
        private readonly IDecumentService decumentsService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<User> userManager;
        public DocumentsController(IDecumentService decumentsService, IWebHostEnvironment env, IMapper mapper, ApiDbContext dbContext, UserManager<User> userManager) : base(env)
        {
            this.decumentsService = decumentsService;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.env = env;
            this.userManager = userManager;

        }
        [HttpPost]
        [Route("AddDocuments")]
        public async Task<ActionResult<NotesApiResult<string>>> AddDocuments(Decuments model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                CaseDetail CurrentCase = new CaseDetail();
                var data = Convert.ToInt32(model.CaseId);
                if (model.CaseId != null)
                {
                    CurrentCase = dbContext.CaseDetail.Find(data);
                }
                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {
                        model.FirmId = GetFirm.Id;
                        if (model.Tokken == "0")
                        {
                            model.Tokken = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            model.Tokken = model.Tokken;
                        }
                        if (model.File != null && model.File.Length > 0)
                        {

                            model.DecumentPath = await SaveFileAsync(model.File, model.extention);
                            model.File = null;

                        }

                        if (model.UserType == "Client" && model.DecumentType != "personal")
                        {
                            if (CurrentCase != null)
                            {
                                model.CaseName = CurrentCase.CaseName;
                                model.CaseNumber = CurrentCase.CaseNumber.ToString();
                            }
                        }

                        int Id = await decumentsService.AddDecuments(model);
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
        [Route("GetDocuments")]
        public async Task<IActionResult> GetDocuments(string UserId)
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
                        List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                        List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.UserId == UserId).ToList();
                        return Ok(_FilterResultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDocumentsDMS")]
        public async Task<IActionResult> GetDocumentsDMS(int Gid, int level)
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
                        List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                        List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.DirectoryId == Gid).ToList();
                        List<Decuments> data = _FilterResultModel.ToList().Where(x => x.DirectoryLevel == level && x.FirmId == CurrentFirm.Id).ToList();
                        List<Decuments> newdata = data.ToList().GroupBy(x => x.Tokken).Select(x=>x.FirstOrDefault()).ToList();
                        return Ok(newdata);
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDocumentsDMSbyVer")]
        public async Task<IActionResult> GetDocumentsDMSbyVer(string token)
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
                        List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                        List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id && x.Tokken == token).ToList();
                        return Ok(_FilterResultModel);
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDocumentsById")]
        public async Task<IActionResult> GetDocumentsById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Decuments model = await decumentsService.GetDecumentsByid(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteDocuments")]
        public async Task<IActionResult> DeleteDocuments(int Id)
        {
            try
            {
                await decumentsService.DeleteDecuments(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region Admin Documents
        [HttpPost]
        [Route("AddAdminDocuments")]
        public async Task<ActionResult<NotesApiResult<string>>> AddAdminDecuments(Decuments model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    model.UserId = getCurrentParent.Id;
                    if (model.File != null && model.File.Length > 0)
                    {

                        model.DecumentPath = await SaveFileAsync(model.File, model.extention);
                        model.File = null;

                    }

                    int Id = await decumentsService.AddDecuments(model);
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
        [Route("GetAdminDocuments")]
        public async Task<IActionResult> GetAdminDecuments(string UserId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var ParentId = loggedInUser != null ? loggedInUser.ParentId : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {

                    List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                    List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.UserId == UserId).ToList();
                    return Ok(_FilterResultModel.ToList());

                }
                return BadRequest("Invalid Admin Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetAdminDocumentsById")]
        public async Task<IActionResult> GetAdminDecumentsById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Decuments model = await decumentsService.GetDecumentsByid(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAdminDocuments")]
        public async Task<IActionResult> DeleteAdminDecuments(int Id)
        {
            try
            {
                await decumentsService.DeleteDecuments(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetDocumentsClient")]
        public async Task<IActionResult> GetDocumentsClient(string UserId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                //var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                        List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.UserId == UserId).ToList();
                        return Ok(_FilterResultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDocumentsStaff")]
        public async Task<IActionResult> GetDocumentsStaff(string UserId)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                //var ParentId = loggedInUser != null ? loggedInUser.Id : null;
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var CurrentFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (CurrentFirm != null)
                    {
                        List<Decuments> _ResultModel = await decumentsService.GetDecuments();
                        List<Decuments> _FilterResultModel = _ResultModel.ToList().Where(x => x.UserId == UserId).ToList();
                        return Ok(_FilterResultModel.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddDocumentsClient")]
        public async Task<ActionResult<NotesApiResult<string>>> AddDocumentsClient(Decuments model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getcaseDetail = dbContext.CaseDetail.ToList().Where(x => x.Id == Convert.ToInt64(model.CaseId)).FirstOrDefault();
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {
                        model.FirmId = GetFirm.Id;
                        if (model.File != null && model.File.Length > 0)
                        {

                            model.DecumentPath = await SaveFileAsync(model.File, model.extention);
                            model.File = null;

                        }
                        if (model.CaseName != null)
                        {
                            model.CaseName = getcaseDetail.CaseName;
                            model.CaseNumber = getcaseDetail.CaseNumber.ToString();
                        }


                        int Id = await decumentsService.AddDecuments(model);
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
        [HttpPost]
        [Route("AddDocumentsStaff")]
        public async Task<ActionResult<NotesApiResult<string>>> AddDocumentsStaff(Decuments model)
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
                        if (model.File != null && model.File.Length > 0)
                        {

                            model.DecumentPath = await SaveFileAsync(model.File, model.extention);
                            model.File = null;

                        }

                        int Id = await decumentsService.AddDecuments(model);
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
        [Route("GetMyClientsDocuments")]
        public async Task<IActionResult> GetMyClientsDocuments()
        {
            try
            {
                var Id = 0;
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();

                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {
                        Id = GetFirm.Id;
                    }
                }

                List<Decuments> model = decumentsService.GetDecuments().Result.ToList().Where(x => x.FirmId == Id && x.UserType == "Client" && x.DecumentType != "Personal").ToList();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        #endregion
    }
}



