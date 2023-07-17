using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Attorney,Customer,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        private readonly ApiDbContext dbContext;
        public TaskController(ITaskService taskService, IActivityService activityService, UserManager<User> userManager, ApiDbContext dbContext)
        {
            this.taskService = taskService;
            this.activityService = activityService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask(Tasks task)
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
                        int Id = await taskService.AddTaskAsync(task);
                        await activityService.AddActivity("Add Task", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a new task", loggedInUser.Id);
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
        [Route("GetTasks")]
        public async Task<IActionResult> GetTasks(string userId = "", string userType = "")
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
                            List<Tasks> tasks = await taskService.GetTasksAsync(userId, userType);
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
        [Route("GetTaskById")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            try
            {
                Tasks task = await taskService.GetTaskByIdAsync(Id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(Tasks task)
        {
            try
            {
                task = await taskService.UpdateTaskAsync(task);
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
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int Id)
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

        [HttpGet]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int Id, string Status)
        {
            try
            {
                await taskService.ChangeStatusAsync(Id, Status);
                return Ok("task updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTasksClient")]
        public async Task<IActionResult> GetTasksClient(string userId = "", string userType = "")
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
                        List<Tasks> tasks = await taskService.GetTasksAsync(userId, userType);
                        return Ok(tasks.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Parent Attorney Account User");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTasksStaff")]
        public async Task<IActionResult> GetTasksStaff(string userId = "", string userType = "")
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
                        List<Tasks> tasks = await taskService.GetTasksAsync(userId, userType);
                        return Ok(tasks.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Parent Attorney Account User");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
