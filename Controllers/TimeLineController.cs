using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLineController : CommonController
    {
        private readonly ITimeLineService timeLineService;
        private readonly IWebHostEnvironment env;
        private readonly IActivityService activityService;
        private readonly UserManager<User> userManager;
        private readonly ApiDbContext dbContext;
        public TimeLineController(ITimeLineService timeLineService, IActivityService activityService, UserManager<User> userManager, IWebHostEnvironment env, ApiDbContext dbContext) : base(env)
        {
            this.timeLineService = timeLineService;
            this.env = env;
            this.activityService = activityService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        [HttpPost]
        [Route("AddTimeLine")]
        public async Task<IActionResult> AddTimeLine([FromForm] TimeLine timeLine)
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
                        timeLine.FirmId = GetFirm.Id;
                        timeLine.IsReminder = false;
                        //if (timeLine.File != null && timeLine.File.Length > 0)
                        //{
                        //    timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        //}
                        if (timeLine.File != null && timeLine.File.Length > 0)
                        {
                            if (timeLine.FileType == "file")
                            {
                                timeLine.FilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "video")
                            {
                                timeLine.VideoFilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "doc")
                            {
                                timeLine.DocFilePath = await SaveFileAsync(timeLine.File);
                            }

                        }
                        timeLine.Id = await timeLineService.AddTimeLineAsync(timeLine);
                        await activityService.AddActivity("Add TimeLine", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {timeLine.CaseName} new timeline", loggedInUser.Id);
                        if (timeLine.Id > 0)
                            return Ok(timeLine);
                        else
                            return BadRequest("Error while adding timeline");
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

        [HttpPost]
        [Route("AddTimeLineStaff")]
        public async Task<IActionResult> AddTimeLineStaff([FromForm] TimeLine timeLine)
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
                        timeLine.FirmId = GetFirm.Id;
                        //if (timeLine.File != null && timeLine.File.Length > 0)
                        //{
                        //    timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        //}
                        if (timeLine.File != null && timeLine.File.Length > 0)
                        {
                            if (timeLine.FileType == "file")
                            {
                                timeLine.FilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "video")
                            {
                                timeLine.VideoFilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "doc")
                            {
                                timeLine.DocFilePath = await SaveFileAsync(timeLine.File);
                            }

                        }



                        timeLine.Id = await timeLineService.AddTimeLineAsync(timeLine);
                        await activityService.AddActivity("Add TimeLine", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {timeLine.CaseName} new timeline", loggedInUser.Id);
                        if (timeLine.Id > 0)
                            return Ok(timeLine);
                        else
                            return BadRequest("Error while adding timeline");
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
        [HttpPost]
        [Route("AddTimeLineClient")]
        public async Task<IActionResult> AddTimeLineClient([FromForm] TimeLine timeLine)
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
                        timeLine.FirmId = GetFirm.Id;

                        //if (timeLine.FileType=="file")
                        //{
                        if (timeLine.File != null && timeLine.File.Length > 0)
                        {
                            if (timeLine.FileType == "file")
                            {
                                timeLine.FilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "video")
                            {
                                timeLine.VideoFilePath = await SaveFileAsync(timeLine.File);
                            }
                            else if (timeLine.FileType == "doc")
                            {
                                timeLine.DocFilePath = await SaveFileAsync(timeLine.File);
                            }

                        }
                        //}
                        //else if (timeLine.FileType == "video")
                        //{
                        //    int type = 1;
                        //    var videoPath = SaveImageMobile(timeLine.VideoFilePathbyte, Guid.NewGuid().ToString(), type);
                        //    timeLine.VideoFilePath = videoPath;
                        //    timeLine.VideoFilePathbyte = null;
                        //}

                        //if (timeLine.File != null && timeLine.File.Length > 0)
                        //{
                        //    timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        //}
                        //if (timeLine.VideoFilePathbyte != null && timeLine.VideoFilePathbyte.Length > 0)
                        //{
                        //    int type = 1;
                        //    var videoPath = SaveImageMobile(timeLine.VideoFilePathbyte, Guid.NewGuid().ToString(), type);
                        //    timeLine.VideoFilePath = videoPath;
                        //    timeLine.VideoFilePathbyte = null;
                        //    //timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        //}
                        timeLine.Id = await timeLineService.AddTimeLineAsync(timeLine);
                        await activityService.AddActivity("Add TimeLine", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {timeLine.CaseName} new timeline", loggedInUser.Id);
                        if (timeLine.Id > 0)
                            return Ok(timeLine);
                        else
                            return BadRequest("Error while adding timeline");
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
        [HttpPost]
        [Route("AddTimeLineClientMobile")]
        public async Task<IActionResult> AddTimeLineClientMobile(TimeLine timeLine)
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
                        timeLine.FirmId = GetFirm.Id;
                        if (timeLine.FilePathbyte != null && timeLine.FilePathbyte.Length > 0)
                        {
                            int type = 1;
                            var docPath = SaveImageMobile(timeLine.FilePathbyte, Guid.NewGuid().ToString(), type);
                            timeLine.FilePath = docPath;
                            timeLine.FilePathbyte = null;
                            //timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        }
                        if (timeLine.VideoFilePathbyte != null && timeLine.VideoFilePathbyte.Length > 0)
                        {
                            int type = 2;
                            var videoPath = SaveImageMobile(timeLine.VideoFilePathbyte, Guid.NewGuid().ToString(), type);
                            timeLine.VideoFilePath = videoPath;
                            timeLine.VideoFilePathbyte = null;
                            //timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        }
                        if (timeLine.DocFilePathbyte != null && timeLine.DocFilePathbyte.Length > 0)
                        {
                            int type = 3;
                            var docPath = SaveImageMobile(timeLine.DocFilePathbyte, Guid.NewGuid().ToString(), type);
                            timeLine.DocFilePath = docPath;
                            timeLine.DocFilePathbyte = null;
                            //timeLine.FilePath = await SaveFileAsync(timeLine.File);
                        }
                        timeLine.Id = await timeLineService.AddTimeLineAsync(timeLine);
                        await activityService.AddActivity("Add TimeLine", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added {timeLine.CaseName} new timeline", loggedInUser.Id);
                        if (timeLine.Id > 0)
                            return Ok(timeLine);
                        else
                            return BadRequest("Error while adding timeline");
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
        [Route("GetTimeLinesByUserId")]
        public async Task<IActionResult> GetTimeLinesByUserId(string userId, string userType = "non-client")
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
                        List<TimeLine> timeLines = await timeLineService.GetTimeLinesAsync(getCurrentParent.Id, userType);
                        return Ok(timeLines.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetTimeLinesByUserIdClient")]
        public async Task<IActionResult> GetTimeLinesByUserIdClient(string userId, string userType = "non-client")
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
                        List<TimeLine> timeLines = await timeLineService.GetTimeLinesAsync(userId, userType);
                        return Ok(timeLines.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetTimeLinesByUserIdStaff")]
        public async Task<IActionResult> GetTimeLinesByUserIdStaff(string userId, string userType = "non-client")
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
                        List<TimeLine> timeLines = await timeLineService.GetTimeLinesAsync(userId, userType);
                        return Ok(timeLines.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList());
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
        [Route("GetTimelinesByUserIdStaff")]
        public async Task<IActionResult> GettimeLinesbyUserIdStaff(int id, string userId, string usertype = "")
        {
            var getdata = dbContext.TimeLine.ToList().Where(x => x.Id == id && x.Comment == usertype).ToList();
            if (getdata != null)
            {
                List<TimeLine> list = await timeLineService.GetTimeLinesAsync(userId, usertype);
                List<TimeLine> lists = await timeLineService.GetTimeLinesAsync(userId, usertype);
                if (list != null && list.Count >= 0)
                {
                    return Ok(list);
                }
                else
                {
                    return Ok(lists.ToList().Where(x => x.HostLink == "").ToList());
                }
            }
            return BadRequest("please add something here first");
        }

        public string SaveImageMobile(byte[] str, string ImgName, int type)
        {
            if (type == 1)
            {
                string webRootPath = env.ContentRootPath;
                string filePath = string.Empty;
                filePath = webRootPath + "\\Images\\";
                //String path = HttpContext.GetServerVariable.MapPath("~/images/");
                string p = filePath.Replace("/", @"\");
                //.Replace("/", @"\");
                string imageName = ImgName + ".jpg";
                //set the image path
                string imgPath = Path.Combine(p, imageName);
                // string[] str = ImgStr.Split(',');
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                var savedpath = "Images/";
                // var savedpath = webRootPath;
                string sp = savedpath.Replace(@"\", "/");
                return sp + imageName;
            }
            else if (type == 2)
            {
                string webRootPath = env.ContentRootPath;
                string filePath = string.Empty;
                filePath = webRootPath + "\\Images\\";
                //String path = HttpContext.GetServerVariable.MapPath("~/images/");
                string p = filePath.Replace("/", @"\");
                //.Replace("/", @"\");
                string imageName = ImgName + ".mp4";
                //set the image path
                string imgPath = Path.Combine(p, imageName);
                // string[] str = ImgStr.Split(',');
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                var savedpath = "Images/";
                // var savedpath = webRootPath;
                string sp = savedpath.Replace(@"\", "/");
                return sp + imageName;
            }
            else
            {
                string webRootPath = env.ContentRootPath;
                string filePath = string.Empty;
                filePath = webRootPath + "\\Images\\";
                //String path = HttpContext.GetServerVariable.MapPath("~/images/");
                string p = filePath.Replace("/", @"\");
                //.Replace("/", @"\");
                string imageName = ImgName + ".pdf";
                //set the image path
                string imgPath = Path.Combine(p, imageName);
                // string[] str = ImgStr.Split(',');
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                var savedpath = "Images/";
                // var savedpath = webRootPath;
                string sp = savedpath.Replace(@"\", "/");
                return sp + imageName;
            }
        }


        [HttpGet]
        [Route("DeleteTimeLine")]
        public async Task<IActionResult> DeleteTimeLine(int ID)
        {
            try
            {
                await timeLineService.DeleteTimeLineAsync(ID);
                return Ok("Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateTimeLine")]
        public async Task<IActionResult> UpdateTimeLine([FromForm] TimeLine timeLine)
        {
            try
            {
                await timeLineService.UpdateTimeLineAsync(timeLine);
                return Ok("Updated Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateTimeLineMobile")]
        public async Task<IActionResult> UpdateTimeLineMobile( TimeLine timeLine)
        {
            try
            {
                await timeLineService.UpdateTimeLineAsync(timeLine);
                return Ok("Updated Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
