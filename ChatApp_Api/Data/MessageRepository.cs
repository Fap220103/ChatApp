using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                        .Include(x => x.Sender)
                        .Include(x => x.Recipient)
                        .SingleOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                                .OrderByDescending(m => m.MessageSent)
                                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username
                                    && u.RecipientDeleted == false), // tin nhan minh da nhan
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username
                                    && u.SenderDeleted == false), // tin nhan minh da gui 
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username 
                                    && u.RecipientDeleted == false
                                    && u.DateRead == null)

            };
            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .Include(u=>u.Sender).ThenInclude(p=>p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
               .Where(
                   m => m.Recipient.UserName == currentUsername && m.RecipientDeleted==false &&
                        m.Sender.UserName == recipientUsername ||
                        m.Recipient.UserName == recipientUsername && m.SenderDeleted == false &&
                        m.Sender.UserName == currentUsername
               )
               .OrderBy(m => m.MessageSent)
               .ToListAsync();


            var unreadMessages = messages.Where(m => m.DateRead == null
                && m.Recipient.UserName == currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
