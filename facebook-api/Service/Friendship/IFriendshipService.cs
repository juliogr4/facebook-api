using facebook_api.Models.DTO.Request.Friendship;
using facebook_api.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Service.Friendship
{
    public interface IFriendshipService
    {
        Task SendRequest(SendRequest sendRequest);
        Task UpdateRequest(UpdateRequest updateRequest);
        Task<List<UserEntity>> GetByStatus(int userID, string status);
    }
}
