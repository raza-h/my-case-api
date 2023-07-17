using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IMessageService
    {
        Task<int> AddMessageAsync(Message message);
        Task<Message> ReadMessage(int Id);
        Task<List<Message>> GetMessagesByUserIdAsync(string Id);
        Task<List<Message>> GetMessagesBySenderIdAndReceiverIdAsync(string senderId, string receiverId, string userId);
        Task<int> AddArchiveAsync(ArchiveContact archiveContact);
        Task UnArchiveAsync(ArchiveContact archiveContact);

        #region Chat Group
        Task<int> AddGroupAsync(ChatGroup chatGroup);
        Task<List<GroupUser>> GetGroupUsersAsync(int groupId);
        Task<List<Message>> GetGroupMessagesAsync(int groupId, string userId);
        Task<ChatGroup> UpdateGroupAsync(ChatGroup chatGroup);
        Task<UserGroupMessage> ReadGroupMessage(string userId, int groupId, int messageId);
        Task AddToGroupAsync(ChatGroup chatGroup);
        Task AddGroupMessage(Message message);
        Task RemoveFromGroupAsync(int groupId, string userId);
        #endregion
    }
}
