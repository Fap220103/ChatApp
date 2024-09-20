using AutoMapper;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Extensions;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_Api.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepository,
                                IUserRepository userRepository,
                                IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();
            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send message to your seft");
            var sender = await _userRepository.GetByUsernameAsync(username);
            var recipient = await _userRepository.GetByUsernameAsync(createMessageDto.RecipientUsername);
            if (recipient == null) return NotFound();
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();
            var messages = await _messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(
                messages.CurrentPage, 
                messages.PageSize, 
                messages.TotalCount,
                messages.TotalPages);
            return messages;
        }
        [HttpGet("thread/{username}")]
        public async Task<IActionResult> GetMessageThread(string username)
        {
            var currentUsername = User.GetUserName();
            return Ok(await _messageRepository.GetMessagesThread(currentUsername, username));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();

            var message = await _messageRepository.GetMessage(id);

            if (message.SenderUserName != username && message.RecipientUserName != username)
                return Unauthorized();

            if (message.SenderUserName == username) message.SenderDeleted = true;
            if (message.RecipientUserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepository.DeleteMessage(message);
            }

            if (await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the message");

        }
    }
}
