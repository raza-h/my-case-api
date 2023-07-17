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
    [Authorize(Roles = "Attorney,Customer,Staff,Client,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService eventsService;
        private readonly IMapper mapper;
        private readonly ApiDbContext dbContext;
        public EventsController(IEventsService eventsService, IMapper mapper, ApiDbContext dbContext)
        {
            this.eventsService = eventsService;
            this.mapper = mapper;
            this.dbContext = dbContext;


        }
        [HttpPost]
        [Route("AddUpdateEvents")]
        public async Task<ActionResult<EventsApiResult<string>>> AddUpdateEvents(Events model)
        {
            try
            {
                //CaseDetail CaseDetailedMap = mapper.Map<CaseDetail>(caseDetail);
                int Id = await eventsService.AddEvents(model);


                return new EventsApiResult<string>
                {
                    Data = string.Empty,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Exception = null
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new EventsApiResult<string>
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
        [Route("GetEvents")]
        public async Task<IActionResult> GetEvents(string userId)
        {
            try
            {
                List<Events> events = await eventsService.GetEvents(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetEventsById")]
        public async Task<IActionResult> GetEventsById(int Id)
        {
            try
            {
                //int _Id = Convert.ToInt32(Id);
                Events model = await eventsService.GetEventsByid(Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateEvents")]
        public async Task<IActionResult> UpdateEvents(Events model)
        {
            try
            {
                model = await eventsService.UpdateEvents(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteEvents")]
        public async Task<IActionResult> DeleteEvents(int Id)
        {
            try
            {
                await eventsService.DeleteEvents(Id);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetWorkflowEvents")]
        public async Task<IActionResult> GetWorkflowEvents(string userId, int id)
        {
            try
            {
                List<Events> events = await eventsService.GetWorkflowEvents(userId,id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


    }
}
