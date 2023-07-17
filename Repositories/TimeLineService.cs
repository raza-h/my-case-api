using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class TimeLineService : ITimeLineService
    {
        private readonly ApiDbContext dbContext;
        private readonly EncryptDecrypt encryptDecrypt;
        public TimeLineService(ApiDbContext dbContext, EncryptDecrypt encryptDecrypt)
        {
            this.dbContext = dbContext;
            this.encryptDecrypt = encryptDecrypt;
        }
        public async Task<int> AddTimeLineAsync(TimeLine timeLine)
        {
            try
            {
                timeLine.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Encrypt(timeLine.Comment) : "";
                timeLine.FilePath = !string.IsNullOrEmpty(timeLine.FilePath) ? encryptDecrypt.Encrypt(timeLine.FilePath) : "";
                timeLine.DocFilePath = !string.IsNullOrEmpty(timeLine.DocFilePath) ? encryptDecrypt.Encrypt(timeLine.DocFilePath) : "";
                timeLine.VideoFilePath = !string.IsNullOrEmpty(timeLine.VideoFilePath) ? encryptDecrypt.Encrypt(timeLine.VideoFilePath) : "";
                timeLine.HostLink = !string.IsNullOrEmpty(timeLine.HostLink) ? encryptDecrypt.Encrypt(timeLine.HostLink) : "";
                timeLine.JoinLink = !string.IsNullOrEmpty(timeLine.JoinLink) ? encryptDecrypt.Encrypt(timeLine.JoinLink) : "";

                await dbContext.TimeLine.AddAsync(timeLine);
                await dbContext.SaveChangesAsync();
                return timeLine.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TimeLine>> GetTimeLinesAsync(string userId, string userType)
        {
            try
            {
                List<TimeLine> timeLines = new List<TimeLine>();
                if (userType == "non-client")
                {
                    timeLines = await (from timeLine in dbContext.TimeLine
                                      // join userAgainstCase in dbContext.UserAgainstCase on timeLine.CaseId equals userAgainstCase.CaseId
                                       join caseDetail in dbContext.CaseDetail on timeLine.CaseId equals caseDetail.Id
                                       join user in dbContext.User on timeLine.UserId equals user.Id
                                       where (timeLine.UserId == userId || timeLine.UserId == userId || user.ParentId == userId)
                                       select new TimeLine { Id = timeLine.Id, Comment = timeLine.Comment, FilePath = timeLine.FilePath,VideoFilePath = timeLine.VideoFilePath,DocFilePath = timeLine.DocFilePath, CaseName = caseDetail.CaseName, UserImagePath = user.ImagePath, UserName = user.FirstName,FirmId=timeLine.FirmId,HostLink=timeLine.HostLink,JoinLink=timeLine.JoinLink }).Distinct().ToListAsync();
                }
                else
                {
                    timeLines = await (from timeLine in dbContext.TimeLine
                                       join caseDetail in dbContext.CaseDetail on timeLine.CaseId equals caseDetail.Id
                                       join contact in dbContext.Contact on caseDetail.BillingContact equals contact.ContactId
                                       join user in dbContext.User on timeLine.UserId equals user.Id
                                       where (contact.UserId == userId || timeLine.UserId == userId)
                                       select new TimeLine { Id = timeLine.Id, Comment = timeLine.Comment, FilePath = timeLine.FilePath,VideoFilePath = timeLine.VideoFilePath, DocFilePath = timeLine.DocFilePath, CaseName = caseDetail.CaseName, UserImagePath = user.ImagePath, UserName = user.FirstName, FirmId = timeLine.FirmId, HostLink = timeLine.HostLink, JoinLink = timeLine.JoinLink }).Distinct().ToListAsync();
                }
                if (timeLines != null && timeLines.Count > 0)
                {
                    timeLines = timeLines.OrderByDescending(x => x.Id).ToList();
                    foreach (var timeLine in timeLines)
                    {
                        timeLine.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Decrypt(timeLine.Comment) : "";
                        timeLine.FilePath = !string.IsNullOrEmpty(timeLine.FilePath) ? encryptDecrypt.Decrypt(timeLine.FilePath) : "";
                        timeLine.DocFilePath = !string.IsNullOrEmpty(timeLine.DocFilePath) ? encryptDecrypt.Decrypt(timeLine.DocFilePath) : "";
                        timeLine.VideoFilePath = !string.IsNullOrEmpty(timeLine.VideoFilePath) ? encryptDecrypt.Decrypt(timeLine.VideoFilePath) : "";
                        timeLine.HostLink = !string.IsNullOrEmpty(timeLine.HostLink) ? encryptDecrypt.Decrypt(timeLine.HostLink) : "";
                        timeLine.JoinLink = !string.IsNullOrEmpty(timeLine.JoinLink) ? encryptDecrypt.Decrypt(timeLine.JoinLink) : "";
                    }
                }
                return timeLines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<List<TimeLine>> GetTimeLinesReminder()
        {
            try
            {
                List<TimeLine> timeLines = new List<TimeLine>();
                
                {
                    timeLines = await (from timeLine in dbContext.TimeLine
                                       select new TimeLine { Id = timeLine.Id, Comment = timeLine.Comment,UserId=timeLine.UserId,CaseId=timeLine.CaseId, FilePath = timeLine.FilePath, FirmId = timeLine.FirmId, HostLink = timeLine.HostLink, JoinLink = timeLine.JoinLink ,MeetingTime=timeLine.MeetingTime}).Distinct().ToListAsync();
                }
                if (timeLines != null && timeLines.Count > 0)
                {
                    timeLines = timeLines.OrderByDescending(x => x.Id).ToList();
                    foreach (var timeLine in timeLines)
                    {
                        timeLine.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Decrypt(timeLine.Comment) : "";
                        timeLine.FilePath = !string.IsNullOrEmpty(timeLine.FilePath) ? encryptDecrypt.Decrypt(timeLine.FilePath) : "";
                        timeLine.HostLink = !string.IsNullOrEmpty(timeLine.HostLink) ? encryptDecrypt.Decrypt(timeLine.HostLink) : "";
                        timeLine.JoinLink = !string.IsNullOrEmpty(timeLine.JoinLink) ? encryptDecrypt.Decrypt(timeLine.JoinLink) : "";
                    }
                }
                return timeLines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TimeLine>> GetTimeLineByID(int id)
        {
            try
            {
                List<TimeLine> timeLines = new List<TimeLine>();
                
                {
                    timeLines = await (from timeLine in dbContext.TimeLine
                                       where (timeLine.Id==id)
                                       select new TimeLine { Id = timeLine.Id, Comment = timeLine.Comment, UserId = timeLine.UserId, CaseId = timeLine.CaseId, FilePath = timeLine.FilePath, FirmId = timeLine.FirmId, HostLink = timeLine.HostLink, JoinLink = timeLine.JoinLink ,MeetingTime=timeLine.MeetingTime}).Distinct().ToListAsync();
                }
                if (timeLines != null && timeLines.Count > 0)
                {
                    timeLines = timeLines.OrderByDescending(x => x.Id).ToList();
                    foreach (var timeLine in timeLines)
                    {
                        timeLine.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Decrypt(timeLine.Comment) : "";
                        timeLine.FilePath = !string.IsNullOrEmpty(timeLine.FilePath) ? encryptDecrypt.Decrypt(timeLine.FilePath) : "";
                        timeLine.HostLink = !string.IsNullOrEmpty(timeLine.HostLink) ? encryptDecrypt.Decrypt(timeLine.HostLink) : "";
                        timeLine.JoinLink = !string.IsNullOrEmpty(timeLine.JoinLink) ? encryptDecrypt.Decrypt(timeLine.JoinLink) : "";
                    }
                }
                return timeLines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<TimeLine> updateTimeLine(TimeLine timeLine)
        {
            try
            {
                timeLine.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Encrypt(timeLine.Comment) : "";
                timeLine.FilePath = !string.IsNullOrEmpty(timeLine.FilePath) ? encryptDecrypt.Encrypt(timeLine.FilePath) : "";
                timeLine.HostLink = !string.IsNullOrEmpty(timeLine.HostLink) ? encryptDecrypt.Encrypt(timeLine.HostLink) : "";
                timeLine.JoinLink = !string.IsNullOrEmpty(timeLine.JoinLink) ? encryptDecrypt.Encrypt(timeLine.JoinLink) : "";

                dbContext.TimeLine.Update(timeLine);
                await dbContext.SaveChangesAsync();
                return timeLine;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteTimeLineAsync(int ID)
        {
            try
            {
                TimeLine timeLine = dbContext.TimeLine.ToList().FirstOrDefault(x => x.Id == ID);
                if (timeLine!=null)
                {
                    dbContext.Entry(timeLine).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateTimeLineAsync(TimeLine timeLine)
        {
            try
            {
                TimeLine timeLinee = dbContext.TimeLine.ToList().FirstOrDefault(x => x.Id == timeLine.CaseId);
                if (timeLinee!=null)
                {
                    timeLinee.Comment = !string.IsNullOrEmpty(timeLine.Comment) ? encryptDecrypt.Encrypt(timeLine.Comment) : "";
                    dbContext.TimeLine.Update(timeLinee);
                    await dbContext.SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
