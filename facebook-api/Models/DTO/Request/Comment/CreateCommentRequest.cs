namespace facebook_api.Models.DTO.Request.Comment
{
    public class CreateCommentRequest
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Comment { get; set; }
        public IFormFile? File { get; set; }
    }
}
