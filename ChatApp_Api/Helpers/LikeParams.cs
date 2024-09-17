namespace ChatApp_Api.Helpers
{
    public class LikeParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }

    }
}
