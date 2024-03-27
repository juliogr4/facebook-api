namespace facebook_api.Models.DTO.Request.Post
{
    public class UpsertPostRequest
    {
        public int UserID { get; set; }
        public string Message { get; set; }
        public IFormFile File { get; set; }
    }
}
