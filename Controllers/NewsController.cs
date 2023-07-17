using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize(Roles = "Attorney,Customer,Admin,Staff,Client")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly INewsService NewsService;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        public NewsController(ApiDbContext dbContext, INewsService NewsService, IActivityService activityService, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.NewsService = NewsService;
            this.activityService = activityService;
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("AddNews")]
        public async Task<IActionResult> AddNews(News model)
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
                        if (ModelState.IsValid)
                        {
                            model.Id = await NewsService.AddNews(model);
                            await activityService.AddActivity("Add News", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a news", loggedInUser.Id);
                            if (model.Id > 0)
                                return Ok(model);
                            else
                                return BadRequest("Error while adding News");
                        }
                        else
                            return BadRequest("Invalid Data");
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
        [Route("GetAllNews")]
        public async Task<IActionResult> GetAllNews()
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
                        List<News> model = await NewsService.GetAllNews();
                        return Ok(model.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
                    }
                    List<News> modela = await NewsService.GetAllNews();
                    return Ok(modela.ToList());
                    //return BadRequest("Please Add Firm Details First");
                }
                return BadRequest("Invalid Attorney Account User");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetNewsById")]
        public async Task<ActionResult<News>> GetNewsById(int Id)
        {
            try
            {
                News model = await NewsService.GetNewsById(Id);
                return model;
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateNews")]
        public async Task<IActionResult> UpdateNews(News model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await NewsService.UpdateNews(model);
                await activityService.AddActivity("Update News", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as news", loggedInUser.Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteNews")]
        public async Task<IActionResult> DeleteNews(int Id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await NewsService.DeleteNews(Id);
                await activityService.AddActivity("Delete News", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted as news", loggedInUser.Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        [Route("AddNewsAdmin")]
        public async Task<IActionResult> AddNewsAdmin(News model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.Id = await NewsService.AddNews(model);
                        await activityService.AddActivity("Add News", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a news", loggedInUser.Id);
                        if (model.Id > 0)
                            return Ok(model);
                        else
                            return BadRequest("Error while adding News");
                    }
                    else
                        return BadRequest("Invalid Data");
                }
                return BadRequest("Invalid Attorney Account User");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       


    }
}
