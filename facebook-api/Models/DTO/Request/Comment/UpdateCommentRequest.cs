namespace facebook_api.Models.DTO.Request.Comment
{
    public class UpdateCommentRequest
    {
        public string Comment { get; set; }
        public IFormFile? File { get; set; }
    }
}
