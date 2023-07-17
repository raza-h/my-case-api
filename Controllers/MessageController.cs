using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Authorize(Roles = "Attorney,Customer,Staff,Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : CommonController
    {
        private readonly IMessageService messageService;
        public MessageController(IWebHostEnvironment env, IMessageService messageService) : base(env)
        {
            this.messageService = messageService;
        }
        #region Message
        [HttpPost]
        [Route("AddMessage")]
        public async Task<IActionResult> AddMessage(Message message)
        {
            try
            {
                if (message.Image != null && message.Image.Length > 0)
                {
                    var imgPath = SaveImage(message.Image, Guid.NewGuid().ToString());
                    message.Image = null;
                    message.ImagePath = imgPath;
                }
                message.Id = await messageService.AddMessageAsync(message);
                if (message.Id > 0 && message.GroupId != null && message.GroupId > 0)
                {
                    await messageService.AddGroupMessage(message);
                    return Ok(message.Id);
                }
                else if (message.Id > 0)
                    return Ok(message.Id);
                else
                    return BadRequest("Error while adding message");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetMessagesByUserId")]
        public async Task<IActionResult> GetMessagesByUserId(string userId)
        {
            try
            {
                var messages = await messageService.GetMessagesByUserIdAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetMessagesBySenderIdAndReceiverId")]
        public async Task<IActionResult> GetMessagesBySenderIdAndReceiverId(string senderId, string receiverId, string userId)
        {
            try
            {
                List<Message> messages = await messageService.GetMessagesBySenderIdAndReceiverIdAsync(senderId, receiverId, userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddArchive")]
        public async Task<IActionResult> AddArchive(ArchiveContact archiveContact)
        {
            try
            {
                archiveContact.Id = await messageService.AddArchiveAsync(archiveContact);
                if (archiveContact.Id > 0)
                    return Ok("Contact added to archive successfully");
                else
                    return BadRequest("Error while adding message");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UnArchive")]
        public async Task<IActionResult> UnArchive(ArchiveContact archiveContact)
        {
            try
            {
                await messageService.UnArchiveAsync(archiveContact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ReadMessage")]
        public async Task<IActionResult> ReadMessage(int Id)
        {
            try
            {
                await messageService.ReadMessage(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Chat Group
        [HttpPost]
        [Route("AddGroup")]
        public async Task<IActionResult> AddGroup(ChatGroup chatGroup)
        {
            try
            {
                if (chatGroup.Image != null && chatGroup.Image.Length > 0)
                {
                    var imgPath = SaveImage(chatGroup.Image, Guid.NewGuid().ToString());
                    chatGroup.Image = null;
                    chatGroup.ImagePath = imgPath;
                }
                int Id = await messageService.AddGroupAsync(chatGroup);
                if (Id > 0)
                {
                    await messageService.AddToGroupAsync(chatGroup);
                    chatGroup.message.SenderId = chatGroup.CreatedBy;
                    chatGroup.message.Id = await messageService.AddMessageAsync(chatGroup.message);
                    chatGroup.message.GroupId = Id;
                    if (chatGroup.message.Id > 0)
                        await messageService.AddGroupMessage(chatGroup.message);
                    return Ok(Id);
                }
                else
                    return BadRequest("Error while adding group");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetGroupUsers")]
        public async Task<IActionResult> GetGroupUsers(int Id)
        {
            try
            {
                var groupUsers = await messageService.GetGroupUsersAsync(Id);
                return Ok(groupUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetGroupMessages")]
        public async Task<IActionResult> GetGroupMessages(int groupId, string userId)
        {
            try
            {
                var messages = await messageService.GetGroupMessagesAsync(groupId, userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ReadGroupMessage")]
        public async Task<IActionResult> ReadGroupMessage(string userId, int groupId, int messageId)
        {
            try
            {
                await messageService.ReadGroupMessage(userId, groupId, messageId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateGroup")]
        public async Task<IActionResult> UpdateGroup(ChatGroup chatGroup)
        {
            try
            {
                await messageService.UpdateGroupAsync(chatGroup);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
