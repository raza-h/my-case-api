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
    public class ContactUsController : Controller
    {

        private readonly IContactUsService contactUsService;
      
        private readonly ApiDbContext dbContext;
        public ContactUsController(IContactUsService contactUsService, ApiDbContext dbContext)
        {
            this.contactUsService = contactUsService;
           
            this.dbContext = dbContext;
        }
     
        [HttpPost]
        [Route("AddContact")]
        public async Task<ActionResult<NotesApiResult<string>>> AddContact(ContactUs model)
        {
            try
            {

                int Id = await contactUsService.AddContactAsync(model);


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
        [Route("GetContact")]
        public async Task<IActionResult> GetContact()
        {
            try
            {
                List<ContactUs> model = await contactUsService.GetContactAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetContactById")]
        public async Task<IActionResult> GetContactById(string Id)
        {
            try
            {
                int _Id = Convert.ToInt32(Id);
                ContactUs model = await contactUsService.GetContactByIdAsync(_Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
     
        [HttpPost]
        [Route("UpdateContact")]
        public async Task<IActionResult> UpdateContact(ContactUs model)
        {
            try
            {
                model = await contactUsService.UpdateContactAsync(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteContact")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            try
            {
                await contactUsService.DeleteContactAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
