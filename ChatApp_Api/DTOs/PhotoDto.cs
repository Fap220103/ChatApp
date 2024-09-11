using ChatApp_Api.Entities;

namespace ChatApp_Api.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
       
    }
}