using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class PricePlanController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly IPricePlanService subscriptionService;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        public PricePlanController(ApiDbContext dbContext, IPricePlanService subscriptionService, IActivityService activityService, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.subscriptionService = subscriptionService;
            this.activityService = activityService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("AddPricePlan")]
        public async Task<IActionResult> AddPricePlan(PricePlan model) 
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                if (ModelState.IsValid)
                {
                    model.PlanID = await subscriptionService.AddPricePlanAsync(model);
                    await activityService.AddActivity("Add Price Plan", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {model.PlanName} new Price Plans", loggedInUser.Id);
                    if (model.PlanID > 0)
                        return Ok(model);
                    else
                        return BadRequest("Error while adding price plan");
                }
                else
                    return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPricePlans")]
        public async Task<IActionResult> GetPricePlans()
        {
            try
            {
                List<PricePlan> pricePlan = await subscriptionService.GetPricePlansAsync();
                return Ok(pricePlan);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetPricePlanById")]
        public async Task<ActionResult<PricePlan>> GetPricePlanById(int Id)
        {
            try
            {
                PricePlan pricePlan = await subscriptionService.GetPricePlanByIdAsync(Id);
                return pricePlan;
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPackageByPlanId")]
        public async Task<IActionResult> GetPackageByPlanId(int Id)
        {
            try
            {
                PackageService packageService = await subscriptionService.GetPackageByPlanIdAsync(Id);
                if (packageService != null)
                    return Ok(packageService);
                else
                    return NotFound("No record found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdatePricePlan")]
        public async Task<IActionResult> UpdatePricePlan(PricePlan pricePlan)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await subscriptionService.UpdatePricePlanAsync(pricePlan);
                await activityService.AddActivity("Update Price Plan", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as Price Plans", loggedInUser.Id);
                return Ok(pricePlan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePricePlan")]
        public async Task<IActionResult> DeletePricePlan(int Id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await subscriptionService.DeletePricePlanAsync(Id);
                await activityService.AddActivity("Delete Price Plan", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted as Price Plans", loggedInUser.Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddService")]
        public async Task<IActionResult> AddService(Service model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = await subscriptionService.AddServiceAsync(model);
                    if (model.Id > 0)
                        return Ok(model);
                    else
                        return BadRequest("Error while adding service");
                }
                else
                    return BadRequest("Invalid Data");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetServices")]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                List<Service> services = await subscriptionService.GetServicesAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetServiceById")]
        public async Task<IActionResult> GetServiceById(int Id)
        {
            try
            {
                Service service = await subscriptionService.GetServiceByIdAsync(Id);
                if (service != null)
                    return Ok(service);
                else
                    return NotFound("No record found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateService")]
        public async Task<IActionResult> UpdateService(Service service)
        {
            try
            {
                await subscriptionService.UpdateServiceAsync(service);
                return Ok(service);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteService")]
        public async Task<IActionResult> DeleteService(int Id)
        {
            try
            {
                await subscriptionService.DeleteServiceAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdatePackage")]
        public async Task<IActionResult> UpdatePackage(PackageService packageService)
        {
            try
            {
                await subscriptionService.UpdatePackageAsync(packageService);
                return Ok("package updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetPackages")]
        public async Task<IActionResult> GetPackages()
        {
            try
            {
                List<PackageService> packageServices = await subscriptionService.GetPackageServices();
                return Ok(packageServices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
