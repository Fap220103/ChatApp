using ChatApp_Api.Data;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Extensions;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.SignalR
{
    public class GroupHub : Hub
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GroupHub(DataContext context,IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        // Tham gia nhóm khi mở giao diện nhóm chat
        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task LeaveGroup(string groupId)
        {
            if (!string.IsNullOrEmpty(groupId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
                await Clients.Group(groupId).SendAsync("UserLeft", Context.User.GetUserName());
            }
        }


        // Gửi tin nhắn trong nhóm
        public async Task SendMessageToGroup(CreateMessageGroupDto groupChat)
        {
            var username = Context.User.GetUserName();
        
            var sender = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            var message = new MessagesChat
            {
                Sender = sender,
                SenderUserName = Context.User.GetUserName(),
                GroupId = groupChat.groupId,
                Content = groupChat.message,
            };
            await _context.MessagesChat.AddAsync(message);
            await _context.SaveChangesAsync();
            await Clients.Group(groupChat.groupId.ToString()).SendAsync("ReceiveGroupMessage", groupChat.message);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var groupId = httpContext.Request.Query["groupId"];

            if (!string.IsNullOrEmpty(groupId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());

                var messagesGroup = await _context.MessagesChat
                    .Where(x => x.GroupId.ToString() == groupId)
                    .ToListAsync();

                await Clients.Caller.SendAsync("ReceiveMessagesGroup", messagesGroup);
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
          
            await base.OnDisconnectedAsync(exception);
        }
    }
}
