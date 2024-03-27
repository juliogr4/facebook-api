using facebook_api.Models.DTO.Request.User;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;

namespace facebook_api.Service.User
{
    public interface IUserService
    {
        Task Create(CreateUserRequest createUserRequest);
        Task<UserEntity> GetByToken(string token, TokenType tokenType);
        Task VerifyEmail(int userID);
        Task ForgotPassword(string email);
    }
}
