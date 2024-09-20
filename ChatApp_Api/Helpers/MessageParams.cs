namespace ChatApp_Api.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
