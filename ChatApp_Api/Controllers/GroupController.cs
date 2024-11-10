using ChatApp_Api.Data;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Controllers
{
    public class GroupController : BaseApiController
    {
        private readonly DataContext _context;

        public GroupController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            var group = new GroupChat
            {
                Name = createGroupDto.GroupName,
                CreatedByUserId = createGroupDto.CreatedBy
            };

            _context.GroupChats.Add(group);
            await _context.SaveChangesAsync();

            // Thêm người tạo vào nhóm
            var groupMember = new GroupMembersChat
            {
                GroupId = group.GroupId,
                UserId = createGroupDto.CreatedBy
            };
            _context.GroupMembersChat.Add(groupMember);
            await _context.SaveChangesAsync();

            return Ok(group);
        }
        [HttpPost("{groupId}/add-members")]
        public async Task<IActionResult> AddMembersToGroup(int groupId, [FromBody] List<int> userIds)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null) return NotFound("Group not found");

            foreach (var userId in userIds)
            {
                var existingMember = await _context.GroupMembersChat
                    .Where(gm => gm.GroupId == groupId && gm.UserId == userId)
                    .FirstOrDefaultAsync();

                if (existingMember == null)
                {
                    var groupMember = new GroupMembersChat
                    {
                        GroupId = groupId,
                        UserId = userId
                    };
                    _context.GroupMembersChat.Add(groupMember);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Members added to group successfully");
        }

    }
}
