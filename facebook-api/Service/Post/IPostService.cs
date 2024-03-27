using facebook_api.Models.DTO.Request.Post;
using facebook_api.Models.DTO.Response.Post;

namespace facebook_api.Service.Post
{
    public interface IPostService
    {
        Task Create(UpsertPostRequest upsertPostRequest);
        Task Update(int postID, UpsertPostRequest upsertPostRequest);
        Task Delete(int postID);
        Task<IEnumerable<PostResponse>> GetFeed(int userID);
        Task<UserProfilePostsResponse> GetUserProfilePosts(int sourceID, int userID);
        Task<bool> CheckFriendshipStatus(int sourceID, int userID);
    }
}
