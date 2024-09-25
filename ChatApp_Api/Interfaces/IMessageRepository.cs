using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Helpers;

namespace ChatApp_Api.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId); 
        void AddMessage(Message message);
        void DeleteMessage(Message message);    
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername,string recipientUsername);
    }
}
