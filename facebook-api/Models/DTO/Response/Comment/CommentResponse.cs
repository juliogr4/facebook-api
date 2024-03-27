namespace facebook_api.Models.DTO.Response.Comment
{
    public class CommentResponse
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Comment { get; set; }
        public string MediaType { get; set; }
        public string FilePath { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // returned
        public int TotalLikes { get; set; }
    }
}
