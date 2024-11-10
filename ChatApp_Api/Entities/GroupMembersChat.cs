namespace ChatApp_Api.Entities
{
    public class GroupMembersChat
    {
        public int GroupId { get; set; } // Khóa ngoại đến Group
        public GroupChat GroupChat { get; set; }

        public int UserId { get; set; } // Khóa ngoại đến User
        public AppUser User { get; set; }
    }
}
