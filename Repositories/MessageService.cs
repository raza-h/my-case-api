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
    public class MessageService : IMessageService
    {
        private readonly ApiDbContext dbContext;
        private readonly EncryptDecrypt encryptDecrypt;
        public MessageService(ApiDbContext dbContext, EncryptDecrypt encryptDecrypt)
        {
            this.dbContext = dbContext;
            this.encryptDecrypt = encryptDecrypt;
        }

        #region Message
        public async Task<int> AddMessageAsync(Message message)
        {
            try
            {
                //for encryption
                message.MessageText = !string.IsNullOrEmpty(message.MessageText) ? encryptDecrypt.Encrypt(message.MessageText) : "";
                message.ImagePath = !string.IsNullOrEmpty(message.ImagePath) ? encryptDecrypt.Encrypt(message.ImagePath) : "";
                await dbContext.Message.AddAsync(message);
                await dbContext.SaveChangesAsync();
                return message.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Message> ReadMessage(int Id)
        {
            try
            {
                Message message = await dbContext.Message.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (message != null)
                {
                    message.ModifiedDate = DateTime.Now;
                    message.IsRead = true;
                    dbContext.Entry(message).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
                return message;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Message>> GetMessagesByUserIdAsync(string Id)
        {
            try
            {
                List<Message> messages = new List<Message>();
                var allMessages = await dbContext.Message.ToListAsync();
                var users = await dbContext.User.ToListAsync();
                var chatGroups = await dbContext.ChatGroup.ToListAsync();
                var archivedContacts = await dbContext.ArchiveContact.ToListAsync();
                var groupUsers = await dbContext.GroupUser.Where(x => x.UserId == Id).ToListAsync();
                var allGroupMessages = await dbContext.UserGroupMessage.ToListAsync();
                var groupMessages = allGroupMessages.Where(x => x.UserId == Id).ToList().OrderByDescending(c => c.CreatedDate).GroupBy(g => g.GroupId).Select(x => x.First()).ToList();
                var messagesSent = allMessages.Where(x => x.SenderId == Id && x.IsGroupMessage == false).ToList().OrderByDescending(x => x.CreatedDate).GroupBy(x => x.ReceiverId).Select(x => x.First()).ToList();
                var messagesReceived = allMessages.Where(x => x.ReceiverId == Id && x.IsGroupMessage == false).ToList().OrderByDescending(x => x.CreatedDate).GroupBy(x => x.SenderId).Select(x => x.First()).ToList();
                if (messagesSent != null && messagesSent.Count > 0)
                {
                    if (messagesReceived != null && messagesReceived.Count > 0)
                    {
                        foreach (var messagesent in messagesSent)
                        {
                            messagesReceived = messagesReceived.Where(x => x.SenderId != messagesent.ReceiverId).ToList();
                        }
                    }
                }
                messages.AddRange(messagesSent);
                messages.AddRange(messagesReceived);
                if (messages != null && messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        string otherContactId = message.SenderId == Id ? message.ReceiverId : message.SenderId;
                        var userId = message.SenderId == Id ? message.ReceiverId : message.SenderId;
                        var user = users.Where(x => x.Id == userId).FirstOrDefault();
                        message.Contact = user != null ? $"{user.FirstName} " : ""; message.Contact += user != null ? !string.IsNullOrEmpty(user.LastName) ? user.LastName : "" : "";
                        message.IsArchived = archivedContacts.Where(x => x.ContactOne == Id && x.ContactTwo == otherContactId).FirstOrDefault() != null ? true : false;
                        message.UnreadCount = allMessages.Where(x => x.SenderId == otherContactId && x.ReceiverId == Id && x.IsRead == false).ToList() != null ?
                                              allMessages.Where(x => x.SenderId == otherContactId && x.ReceiverId == Id && x.IsRead == false).ToList().Count : 0;
                        message.UserImagePath = user != null ? $"{user.ImagePath}" : "";
                    }
                }
                // get group messages for chat
                if (groupMessages != null && groupMessages.Count > 0)
                {
                    foreach (var groupMessage in groupMessages)
                    {
                        Message message = allMessages.Where(x => x.Id == groupMessage.MessageId).FirstOrDefault();
                        if (message != null)
                        {
                            message.Contact = chatGroups.Where(x => x.Id == groupMessage.GroupId).FirstOrDefault() != null ? chatGroups.Where(x => x.Id == groupMessage.GroupId).FirstOrDefault().GroupName : "";
                            message.UserImagePath = chatGroups.Where(x => x.Id == groupMessage.GroupId).FirstOrDefault() != null ? chatGroups.Where(x => x.Id == groupMessage.GroupId).FirstOrDefault().ImagePath : "";
                            message.GroupId = groupMessage.GroupId;
                            message.IsArchived = groupUsers.Where(x => x.GroupId == groupMessage.GroupId && x.UserId == Id).FirstOrDefault() != null ? groupUsers.Where(x => x.GroupId == groupMessage.GroupId && x.UserId == Id).FirstOrDefault().IsArchive : false;
                            message.UnreadCount = allGroupMessages.Where(x => x.UserId == Id && x.UserId != message.SenderId && x.GroupId == groupMessage.GroupId && x.IsRead == false).ToList() != null ?
                                                  allGroupMessages.Where(x => x.UserId == Id && x.UserId != message.SenderId && x.GroupId == groupMessage.GroupId && x.IsRead == false).ToList().Count : 0;
                            messages.Add(message);
                        }
                    }
                }
                DecryptMessages(messages);
                messages = messages.OrderByDescending(x => x.CreatedDate).ToList();
                return messages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Message>> GetMessagesBySenderIdAndReceiverIdAsync(string senderId, string receiverId, string userId)
        {
            try
            {
                List<Message> messages = new List<Message>();
                var allMessages = await dbContext.Message.ToListAsync();
                var users = await dbContext.User.ToListAsync();
                string otherUserId = senderId == userId ? receiverId : senderId;
                var messagesByMe = allMessages.Where(x => x.SenderId == userId && x.ReceiverId == otherUserId).ToList();
                var messagesByOther = allMessages.Where(x => x.SenderId == otherUserId && x.ReceiverId == userId).ToList();
                if (messagesByOther != null && messagesByOther.Count > 0)
                {
                    foreach (var message in messagesByOther)
                    {
                        var user = users.Where(x => x.Id == otherUserId).FirstOrDefault();
                        message.UserImagePath = user != null ? $"{user.ImagePath}" : "";
                    }
                    messagesByOther.ForEach(x => x.IsRead = true);
                    dbContext.Message.UpdateRange(messagesByOther);
                    await dbContext.SaveChangesAsync();
                }
                messages.AddRange(messagesByMe);
                messages.AddRange(messagesByOther);
                messages = messages.OrderBy(x => x.CreatedDate).ToList();
                messages = DecryptMessages(messages);
                return messages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddArchiveAsync(ArchiveContact archiveContact)
        {
            try
            {
                // to archive group
                if (archiveContact != null && archiveContact.GroupId > 0)
                {
                    var groupUser = await dbContext.GroupUser.Where(x => x.GroupId == archiveContact.GroupId && x.UserId == archiveContact.ContactOne).FirstOrDefaultAsync();
                    groupUser.IsArchive = true;
                    dbContext.Entry(groupUser).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return groupUser.Id;
                }
                else // to archive individual chat
                {
                    await dbContext.ArchiveContact.AddAsync(archiveContact);
                    await dbContext.SaveChangesAsync();
                    return archiveContact.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UnArchiveAsync(ArchiveContact archiveContact)
        {
            try
            {
                // to unarchive group
                if (archiveContact != null && archiveContact.GroupId != null && archiveContact.GroupId > 0)
                {
                    var groupUser = await dbContext.GroupUser.Where(x => x.GroupId == archiveContact.GroupId && x.UserId == archiveContact.ContactOne).FirstOrDefaultAsync();
                    groupUser.IsArchive = false;
                    dbContext.Entry(groupUser).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
                else // to unarchive individual chat
                {
                    var contact = await dbContext.ArchiveContact.Where(x => x.ContactOne == archiveContact.ContactOne && x.ContactTwo == archiveContact.ContactTwo).FirstOrDefaultAsync();
                    dbContext.Entry(contact).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Chat Group
        public async Task<List<GroupUser>> GetGroupUsersAsync(int groupId)
        {
            try
            {
                var users = await dbContext.User.ToListAsync();
                var groupUsers = await dbContext.GroupUser.Where(x => x.GroupId == groupId).ToListAsync();
                if (groupUsers != null && groupUsers.Count > 0)
                {
                    foreach (var groupUser in groupUsers)
                    {
                        groupUser.UserImagePath = users.Where(x => x.Id == groupUser.UserId).FirstOrDefault() != null ? users.Where(x => x.Id == groupUser.UserId).FirstOrDefault().ImagePath : "";
                    }
                }
                return groupUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Message>> GetGroupMessagesAsync(int groupId, string userId)
        {
            try
            {
                List<Message> messages = new List<Message>();
                var allMessages = await dbContext.Message.ToListAsync();
                var users = await dbContext.User.ToListAsync();
                var userGroupMessages = await dbContext.UserGroupMessage.Where(x => x.GroupId == groupId).ToListAsync();
                if (userGroupMessages != null && userGroupMessages.Count > 0)
                {
                    foreach (var userGroupMessage in userGroupMessages)
                    {
                        Message message = allMessages.Where(x => x.Id == userGroupMessage.MessageId).FirstOrDefault();
                        var user = users.Where(x => x.Id == message.SenderId).FirstOrDefault();
                        message.UserImagePath = user != null ? $"{user.ImagePath}" : "";
                        messages.Add(message);
                    }
                    userGroupMessages.ForEach(x => x.IsRead = true);
                    dbContext.UserGroupMessage.UpdateRange(userGroupMessages);
                    await dbContext.SaveChangesAsync();
                }
                messages = messages.OrderBy(x => x.CreatedDate).Distinct().ToList();
                messages = DecryptMessages(messages);
                return messages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddGroupAsync(ChatGroup group)
        {
            try
            {
                await dbContext.ChatGroup.AddAsync(group);
                await dbContext.SaveChangesAsync();
                return group.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ChatGroup> UpdateGroupAsync(ChatGroup group)
        {
            try
            {
                List<GroupUser> groupUsers = new List<GroupUser>();
                if (group != null && group.Id > 0 && group.UserIds != null && group.UserIds.Count > 0)
                {
                    var existingGroupUser = await dbContext.GroupUser.Where(x => x.GroupId == group.Id).ToListAsync();
                    dbContext.GroupUser.RemoveRange(existingGroupUser);
                    foreach (var user in group.UserIds)
                    {
                        GroupUser groupUser = new GroupUser();
                        groupUser.GroupId = group.Id;
                        groupUser.UserId = user;
                        groupUsers.Add(groupUser);
                    }
                    await dbContext.GroupUser.AddRangeAsync(groupUsers);
                }
                dbContext.Entry(group).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return group;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserGroupMessage> ReadGroupMessage(string userId, int groupId, int messageId)
        {
            try
            {
                UserGroupMessage message = await dbContext.UserGroupMessage.Where(x => x.UserId == userId && x.GroupId == groupId && x.MessageId == messageId).FirstOrDefaultAsync();
                if (message != null)
                {
                    message.ModifiedDate = DateTime.Now;
                    message.IsRead = true;
                    dbContext.Entry(message).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
                return message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddToGroupAsync(ChatGroup group)
        {
            try
            {
                List<GroupUser> groupUsers = new List<GroupUser>();
                if (group != null && group.Id > 0 && group.UserIds != null && group.UserIds.Count > 0)
                {
                    foreach (var user in group.UserIds)
                    {
                        GroupUser groupUser = new GroupUser();
                        groupUser.GroupId = group.Id;
                        groupUser.UserId = user;
                        groupUsers.Add(groupUser);
                    }
                    await dbContext.GroupUser.AddRangeAsync(groupUsers);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddGroupMessage(Message message)
        {
            try
            {
                List<UserGroupMessage> userGroupMessages = new List<UserGroupMessage>();
                List<GroupUser> groupUsers = await dbContext.GroupUser.Where(x => x.GroupId == message.GroupId).ToListAsync();
                if (groupUsers != null && groupUsers.Count > 0)
                {
                    foreach (var groupUser in groupUsers)
                    {
                        UserGroupMessage userGroupMessage = new UserGroupMessage();
                        userGroupMessage.CreatedDate = DateTime.Now;
                        userGroupMessage.GroupId = message.GroupId.Value;
                        userGroupMessage.UserId = groupUser.UserId;
                        userGroupMessage.MessageId = message.Id;
                        userGroupMessage.ModifiedDate = DateTime.Now;
                        userGroupMessages.Add(userGroupMessage);
                    }
                    await dbContext.UserGroupMessage.AddRangeAsync(userGroupMessages);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task RemoveFromGroupAsync(int groupId, string userId)
        {
            try
            {
                GroupUser groupUser = await dbContext.GroupUser.Where(x => x.GroupId == groupId && x.UserId == userId).FirstOrDefaultAsync();
                dbContext.Entry(groupUser).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private functions
        private List<Message> DecryptMessages(List<Message> messages)
        {
            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    message.MessageText = !string.IsNullOrEmpty(message.MessageText) ? encryptDecrypt.Decrypt(message.MessageText) : "";
                    message.ImagePath = !string.IsNullOrEmpty(message.ImagePath) ? encryptDecrypt.Decrypt(message.ImagePath) : "";
                }
            }
            return messages;
        }
        private List<Message> EncryptMessages(List<Message> messages)
        {
            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    message.MessageText = !string.IsNullOrEmpty(message.MessageText) ? encryptDecrypt.Encrypt(message.MessageText) : "";
                }
            }
            return messages;
        }
        #endregion
    }
}
