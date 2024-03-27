using facebook_api.Models.DTO.Request.User;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;

namespace facebook_api.Repository.User
{
    public interface IUserRepository
    {
        Task Create(UserEntity user);
        Task<UserEntity> GetByEmail(string email);
        Task<UserEntity> GetByToken(string token, TokenType tokenType);
        Task VerifyEmail(int userID);
    }
}
