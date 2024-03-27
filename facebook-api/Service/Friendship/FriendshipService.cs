using facebook_api.Models.DTO.Request.Friendship;
using facebook_api.Models.Entities;
using facebook_api.Repository.Friendship;

namespace facebook_api.Service.Friendship
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        public async Task<List<UserEntity>> GetByStatus(int userID, string status)
        {
            return await _friendshipRepository.GetByStatus(userID, status);
        }

        public async Task SendRequest(SendRequest sendRequest)
        {
            await _friendshipRepository.SendRequest(sendRequest);
        }

        public async Task UpdateRequest(UpdateRequest updateRequest)
        {
            await _friendshipRepository.UpdateRequest(updateRequest);
        }
    }
}
