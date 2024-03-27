namespace facebook_api.Models.DTO.Response.Post
{
    public class PostResponse
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string MediaType { get; set; }
        public string FilePath { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // returned
        public int TotalLikes { get; set; }
        public string UserLastLike { get; set; }
        public int TotalComments { get; set; }
    }
}
