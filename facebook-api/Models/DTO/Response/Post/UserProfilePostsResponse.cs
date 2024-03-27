namespace facebook_api.Models.DTO.Response.Post
{
    public class UserProfilePostsResponse
    {
        public bool AreFriends { get; set; }
        public IEnumerable<PostResponse> Posts { get; set; }
    }
}
