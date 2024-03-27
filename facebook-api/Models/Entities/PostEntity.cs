namespace facebook_api.Models.Entities
{
    public class PostEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string MediaType { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // returned
        public int TotalLikes { get; set; }
        public string UserLastLike { get; set; }
        public int TotalComments { get; set; }
    }
}
