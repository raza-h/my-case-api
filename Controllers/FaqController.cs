using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : Controller
    {
        private readonly IFaqServices faqService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        public FaqController(IFaqServices faqService, IMapper mapper, ApiDbContext dbContext)
        {
            this.faqService = faqService;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddFaqs")]

        public async Task<ActionResult<NotesApiResult<string>>> AddFaqs(Faq model)
        {
            try
            {
        
                int Id = await faqService.AddFaqAsync(model);

              
                return new NotesApiResult<string>
                {
                    Data = string.Empty,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Exception = null
                };
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
        [Route("GetFaqs")]
        public async Task<IActionResult> GetFaqs()
        {
            try
            {
                List<Faq> model = await faqService.GetFaqAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetFaqsById")]
        public async Task<IActionResult> GetFaqsById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                Faq model = await faqService.GetFaqByIdAsync(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("UpdateFaqs")]
        public async Task<IActionResult> UpdateFaqs(Faq model)
        {
            try
            {
                model = await faqService.UpdateFaqAsync(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteFaqs")]
        public async Task<IActionResult> DeleteFaqs(int Id)
        {
            try
            {
                await faqService.DeleteFaqAsync(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
