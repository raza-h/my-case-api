using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Attorney,Customer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService AdminActivityService;
        public ActivityController(IActivityService AdminActivityService)
        {
            this.AdminActivityService = AdminActivityService;
        }

        [HttpGet]
        [Route("GetActivity")]
        public async Task<IActionResult> GetActivity(bool IsFilterByDates, DateTime? Date1 = null, DateTime? Date2 = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<AdminActivity> adminActivities = new List<AdminActivity>();
                if (Date1 < Date2)
                    adminActivities = await AdminActivityService.GetAdminActivities(userId, IsFilterByDates, Date1, Date2);
                else
                    adminActivities = await AdminActivityService.GetAdminActivities(userId, IsFilterByDates, Date2, Date1);
                return Ok(adminActivities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
