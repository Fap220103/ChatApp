using System.ComponentModel.DataAnnotations;

namespace ChatApp_Api.Entities
{
    public class GroupChat
    {
        [Key]
        public int GroupId { get; set; } // Khóa chính của nhóm
        public string Name { get; set; } // Tên nhóm

        public int CreatedByUserId { get; set; } // ID của người tạo nhóm
        public AppUser CreatedByUser { get; set; } // Thông tin người tạo nhóm

        public ICollection<GroupMembersChat> GroupMembers { get; set; } = new List<GroupMembersChat>(); // Các thành viên trong nhóm
        public ICollection<MessagesChat> Messages { get; set; } = new List<MessagesChat>(); // Tin nhắn trong nhóm
    }
}
