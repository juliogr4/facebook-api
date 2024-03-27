using facebook_api.Models.DTO.Request.Friendship;
using facebook_api.Models.Entities;

namespace facebook_api.Repository.Friendship
{
    public interface IFriendshipRepository
    {
        Task SendRequest(SendRequest sendRequest);
        Task UpdateRequest(UpdateRequest updateRequest);
        Task<List<UserEntity>> GetByStatus(int userID, string status);
    }
}
