using facebook_api.Models.Entities;

namespace facebook_api.Repository.Post
{
    public interface IPostRepository
    {
        Task Upsert(PostEntity post);
        Task Delete(int postID);
        Task<IEnumerable<PostEntity>> GetFeed(int userID);
        Task<PostEntity> GetByID(int postID);
        Task<IEnumerable<PostEntity>> GetUserProfilePosts(int userID);
        Task<bool> CheckFriendshipStatus(int sourceID, int userID);
    }
}
