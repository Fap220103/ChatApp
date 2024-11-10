namespace ChatApp_Api.Entities
{
    public class MessagesChat
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public AppUser Sender { get; set; }

        public int? GroupId { get; set; } // ID của nhóm chat nếu là tin nhắn trong nhóm
        public GroupChat Group { get; set; }  // Tham chiếu đến nhóm chat

        public string Content { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    }
}
