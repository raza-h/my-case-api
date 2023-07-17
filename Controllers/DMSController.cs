using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class DMSController : CommonController
    {
        private readonly IDMSService dMSService;
        private readonly IMapper mapper;
        private readonly IActivityService activityService;
        private readonly ApiDbContext dbContext;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<User> userManager;
        public DMSController(IDMSService dMSService, IWebHostEnvironment env, IMapper mapper, ApiDbContext dbContext, UserManager<User> userManager) : base(env)
        {
            this.dMSService = dMSService;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.env = env;
            this.userManager = userManager;

        }


        [HttpPost]
        [Route("AddDirectory")]
        public async Task<IActionResult> AddDirectory(DocSub1Folder model)
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
                        model.FirmId = GetFirm.Id;
                        int Id = 0;
                        if (model.DirectoryType == "Main")
                        {
                            DocumentFolder documentFolder = new DocumentFolder();
                            documentFolder.Name = model.Name;
                            documentFolder.FirmId = model.FirmId;
                            documentFolder.CreatedDate = DateTime.Now.Date;
                            documentFolder.CreatedBy = loggedInUser.FirstName + loggedInUser.LastName;
                            Id = await dMSService.AddDirectoryAsync(documentFolder);
                        }
                        else if (model.DirectoryType == "Sub")
                        {
                            DocSub1Folder docSub1Folder = new DocSub1Folder();
                            docSub1Folder.Name = model.Name;
                            docSub1Folder.DocFolderId = model.DocFolderId;
                            docSub1Folder.FirmId = model.FirmId;
                            Id = await dMSService.AddSubDirectoryAsync(docSub1Folder);
                        }
                        //await activityService.AddActivity("Directory Add", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} added a new Directory Folder", loggedInUser.Id);
                        if (Id > 0)
                            return Ok("Directory added successfully");
                        else
                            return BadRequest("Error while adding Directory");
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
        [Route("GetFolders")]
        public async Task<IActionResult> GetFolders()
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
                        List<DocumentFolder> documentFolders = await dMSService.GetFolderAsync();
                        var data = documentFolders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();

                        List<DocSub1Folder> docSub1Folders = await dMSService.GetFolder1Async();
                        var data1 = docSub1Folders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();


                        List<DocumentFolder> dataFolders = new List<DocumentFolder>();
                        if (data != null)
                        {
                            dataFolders = data;
                            for (int i = 0; i < dataFolders.Count(); i++)
                            {
                                if (dataFolders.ElementAt(i).DocSub1Folders!=null)
                                {
                                    for (int j = 0; j < dataFolders.ElementAt(i).DocSub1Folders.Count(); j++)
                                    {
                                        dataFolders.ElementAt(i).DocSub1Folders.ElementAt(j).DocumentFolder = null;
                                    }
                                }
                                
                            }
                            //if (data1 != null)
                            //{
                            //dataFolders.ElementAt(0).DocSub1Folders = data1;

                            //}
                            //else
                            //{
                            //    dataFolders.ElementAt(0).DocSub1Folders = null;
                            //}
                        }
                        return Ok(dataFolders);
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
        [Route("GetFolderById")]
        public async Task<IActionResult> GetFolderById(int Id)
        {
            try
            {
                DocumentFolder documentFolder = await dMSService.GetFolderByIdAsync(Id);
                return Ok(documentFolder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateFolder")]
        public async Task<IActionResult> UpdateFolder(DMSNewEditValidate editData)
        {
            try
            {
                var data = await dMSService.UpdateFolderAsync(editData);
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await activityService.AddActivity("Update Folder", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} updated as Folder", loggedInUser.Id);
                return Ok("Folder updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteFolder")]
        public async Task<IActionResult> DeleteFolder(int Id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                await dMSService.DeleteFolderAsync(Id);
                await activityService.AddActivity("Delete Folder", $"{loggedInUser.FirstName} {(!string.IsNullOrEmpty(loggedInUser.LastName) ? loggedInUser.LastName : "")} deleted as Folder", loggedInUser.Id);
                return Ok("Folder Deleted Sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllFolders")]
        public async Task<IActionResult> GetAllFolders()
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
                        List<DocumentFolder> documentFolders = await dMSService.GetFolderAsync();
                        var data = documentFolders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();

                        List<DocSub1Folder> docSub1Folders = await dMSService.GetFolder1Async();
                        var data1 = docSub1Folders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        documentFolders.ElementAt(0).DocSub1Folders = data1;

                        List<DocSub2Folder> docSub2Folders = await dMSService.GetFolder2Async();
                        var data2 = docSub2Folders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        //documentFolders.ElementAt(0).DocSub2Folders = data2;

                        List<DocSub3Folder> docSub3Folders = await dMSService.GetFolder3Async();
                        var data3 = docSub3Folders.ToList().Where(x => x.FirmId == CurrentFirm.Id).ToList();
                        //documentFolders.ElementAt(0).DocSub3Folders = data3;
                        return Ok();
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
    }
}
