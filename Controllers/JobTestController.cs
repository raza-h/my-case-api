using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    public class JobTestController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public JobTestController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient,ApiDbContext dbContext, IRecurringJobManager recurringJobManager)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            this.dbContext = dbContext;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
           
            //var less = doubledata.Value.AddMinutes(-20);
            //_backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(),TimeSpan.FromDays(5));
            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public IActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
           
                //_backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.DelayedJob());
                return Ok();
        }
    }
}
