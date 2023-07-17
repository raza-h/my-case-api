using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : CommonController
    {
        private readonly ITaskService taskService;
        private readonly IActivityService activityService;
        private readonly IWorkflowService workflowService;
        private readonly IDecumentService decumentsService;
        private readonly UserManager<User> userManager;
        private readonly ApiDbContext dbContext;
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;
        public WorkflowController(ITaskService taskService, IActivityService activityService, UserManager<User> userManager, ApiDbContext dbContext, IWorkflowService workflowService, IDecumentService decumentsService, IWebHostEnvironment env, IMapper mapper) : base(env)
        {
            this.taskService = taskService;
            this.activityService = activityService;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.workflowService = workflowService;
            this.decumentsService = decumentsService;
            this.env = env;
            this.mapper = mapper;
        }

        #region WorkFlow Task

        [HttpPost]
        [Route("AddWorkflowTask")]
        public async Task<IActionResult> AddWorkflowTask(Tasks task)
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
                        task.FirmId = GetFirm.Id;
                        int Id = await taskService.AddWorkflowTaskAsync(task);
                        await activityService.AddActivity("Add Task", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a new task to workflow", loggedInUser.Id);
                        if (Id > 0)
                            return Ok("task added successfully");
                        else
                            return BadRequest("Error while adding task");
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
        [Route("GetAllWorkflows")]
        public async Task<IActionResult> GetAllWorkflows()
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
                        //List<Tasks> tasks = await taskService.GetTasksAsync(userId, userType);
                        List<WorkflowBase> tasks = await workflowService.GetWorkflowAsync();
                        return Ok(tasks.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetWorkflowById")]
        public async Task<IActionResult> GetWorkflowById(int Id)
        {
            try
            {
                WorkflowBase data = await workflowService.GetWorkflowByIdAsync(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateWorkflowTask")]
        public async Task<IActionResult> UpdateWorkflowTask(Tasks task)
        {
            try
            {
                task = await taskService.UpdateWorkflowTaskAsync(task);
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await activityService.AddActivity("Update Task", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as task", loggedInUser.Id);
                return Ok("task updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteWorkflowTask")]
        public async Task<IActionResult> DeleteWorkflowTask(int Id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await taskService.DeleteTaskAsync(Id);
                await activityService.AddActivity("Delete Task", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted as task", loggedInUser.Id);
                return Ok("Task Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region WorkFlow Documents

        [HttpPost]
        [Route("AddWorkflowDocuments")]
        public async Task<ActionResult<NotesApiResult<string>>> AddWorkflowDocuments(Decuments model)
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

        [HttpDelete]
        [Route("DeleteWorkflowDocuments")]
        public async Task<IActionResult> DeleteWorkflowDocuments(int Id)
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

        #endregion

        #region Workflow Attach to Case

        [HttpPost]
        [Route("AttachWorkflowToCase")]
        public async Task<IActionResult> AttachWorkflowToCase(WorkflowAttach data)
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
                        var getcaseData=dbContext.CaseDetail.Where(x => x.Id==data.CaseId).FirstOrDefault();
                        data.FirmId = GetFirm.Id;
                        data.UserId=getCurrentParent.Id;
                        int Id = await workflowService.AttachWorkflowToCaseAsync(data);
                        await activityService.AddActivity("Attach Workflow", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} attached workflow to {getcaseData.CaseName}", loggedInUser.Id);
                        if (Id > 0)
                            return Ok("Workflow Attached successfully");
                        else if (Id == 0)
                            return Ok("Workflow Already Attached");
                        else
                            return BadRequest("Error while attaching workflow");
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

        #endregion
    }
}
