using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [ApiController]
    public class FirmController : ControllerBase
    {
        private readonly IFirmService firmService;
        public readonly ApiDbContext dbContext;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        public FirmController (IFirmService firmService, ApiDbContext dbContext, IActivityService activityService, UserManager<User> userManager)
        {
            this.firmService = firmService;
            this.dbContext = dbContext;
            this.activityService = activityService;
            this.userManager = userManager;

        }
        [HttpPost]
        [Route("AddFirm")]
        public async Task<IActionResult> AddFirm(Firm firm)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var existingFirm = await dbContext.Company.Where(x => x.Email == firm.FirmEmail).FirstOrDefaultAsync();
                if (existingFirm == null)
                {
                    firm = await firmService.AddFirmAsync(firm);
                    await activityService.AddActivity("Add Firm", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a new Firm", loggedInUser.Id);
                    if (firm.Id > 0)
                    {
                        return Ok("Firm Added Sucessfully");
                    }
                        else
                            return BadRequest("Error Occurred while adding firm");
                }
                else
                    return BadRequest("Firm with this email already exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateFirm")]
        public async Task<IActionResult> UpdateFirm(Firm firm)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                firm = await firmService.UpdateFirmAsync(firm);
                await activityService.AddActivity("Update Firm", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated {firm.FirmName} Firm", loggedInUser.Id);
                return Ok("Firm Updated Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetFirm")]
        public async Task<IActionResult> GetFirm()
        {
            try
            {
                List<Firm> firm = await firmService.GetFirmsAsync();
                return Ok(firm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetFirmByUserId")]
        public async Task<IActionResult> GetFirmByUserId(string userId)
        {
            try
            {
                Firm firm = await firmService.GetFirmByUserIdAsync(userId);
                if (firm != null)
                    return Ok(firm);

                else if (firm == null)
                {
                    var email = User.FindFirstValue(ClaimTypes.Email);
                    var loggedInUser = await userManager.FindByEmailAsync(email);
                    firm = await firmService.GetFirmByUserIdAsync(loggedInUser.ParentId);
                    if (firm != null)
                        return Ok(firm);
                    else
                        return NotFound("Please Add Firm Details First");
                }
                else
                    return NotFound("Please Add Firm Details First");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
