namespace facebook_api.Models.Entities
{
    public class CommentEntity
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Comment { get; set; }
        public string MediaType { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // returned
        public int TotalLikes { get; set; }
    }
}

