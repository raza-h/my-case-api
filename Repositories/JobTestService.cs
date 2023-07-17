using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MyCaseApi.Repositories
{
    public class JobTestService: IJobTestService
    {
        private readonly ApiDbContext dbContext;
        private readonly EmailService emailService;
        private readonly ITimeLineService timeLineService;

        public JobTestService(ApiDbContext dbContext, EmailService emailService, ITimeLineService timeLineService)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
            this.timeLineService = timeLineService;
        }

        public string FireAndForgetJob()
        {
            Debug.WriteLine("Hello from a Fire and Forget job!");
            return "sad";
        }
        public async Task<IActionResult> ReccuringJob()
        {
            List<TimeLine> getdata = await timeLineService.GetTimeLinesReminder();

            for (int i = 0; i < getdata.Count(); i++)
            {
                if (getdata[i].HostLink != null)
                {
                    var doubledata = getdata[i].MeetingTime;
                    if (doubledata.Value.Date == DateTime.Now.Date)
                    {
                        var less = doubledata.Value.AddMinutes(-30);
                        var currentDate = DateTime.Now;

                        var lessMinutes = less.Minute;
                        var lessHours = less.Hour;

                        var currentMinutes = currentDate.Minute;
                        var currentHour = currentDate.Hour;

                        if (lessHours == currentHour && lessMinutes == currentMinutes)
                        {
                            string loginwebUrl = "";
                            bool isEmailSent = emailService.SendReminderEmail(getdata[i].UserId, "", loginwebUrl);
                            if (isEmailSent)
                            {
                                List<TimeLine> updateData = await timeLineService.GetTimeLineByID(getdata[i].Id);
                                for (int j = 0; j < updateData.Count(); j++)
                                {
                                    updateData[j].IsReminder = true;
                                    await timeLineService.updateTimeLine(updateData[j]);
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }
        public void DelayedJob()
        {
            //int id = 11;
            //var getdata = dbContext.TimeLine.ToList().Where(x => x.Id == id).FirstOrDefault();
            //if (getdata != null)
            //{
            //    if (getdata.MeetingTime.Value.)
            //    {

            //    }
            //}
            Debug.WriteLine("Hello from a Delayed job after 3 min!");
        }
        public void ContinuationJob()
        {
            Debug.WriteLine("Hello from a Continuation job!");
        }
    }
}
