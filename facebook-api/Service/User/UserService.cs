using facebook_api.Models.DTO.Request.Email;
using facebook_api.Models.DTO.Request.User;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Repository.User;
using facebook_api.Service.Email;
using facebook_api.Service.Hash;
using Microsoft.AspNetCore.Http.HttpResults;

namespace facebook_api.Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashing _passwordHashing;
        private readonly IEmailService _emailService;
        public UserService(
            IUserRepository userRepository, 
            IPasswordHashing passwordHashing,
            IEmailService emailService
        )
        {
            _userRepository = userRepository;
            _passwordHashing = passwordHashing;
            _emailService = emailService;
        }

        public async Task Create(CreateUserRequest createUserRequest)
        {
            var user = await _userRepository.GetByEmail(createUserRequest.Email);
            if (user != null) { throw new Exception("User already exists"); }
            if(user == null)
            {
                var hash = _passwordHashing.GenerateHash(createUserRequest.Password);
                var token = Guid.NewGuid().ToString();

                await _userRepository.Create(new UserEntity
                { 
                    EmailToken = token,
                    Hash = hash,
                    Email = createUserRequest.Email,
                    FirstName = createUserRequest.FirstName,
                    LastName = createUserRequest.LastName,
                    DOB = createUserRequest.DOB,
                });

                var emailMessage = new EmailMessage(new List<string> { createUserRequest.Email }, "Email confirmation", "email-confirmation.html");
                emailMessage.Parameters.Add("firstName", createUserRequest.FirstName);
                emailMessage.Parameters.Add("link", $"https://localhost:7284/users/verify-email/{token}");
                _emailService.SendEmail(emailMessage);
            }
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user != null)
            {
                var token = Guid.NewGuid().ToString();
                var emailMessage = new EmailMessage(new List<string> { email }, "Reset password", "forgot-password.html");
                emailMessage.Parameters.Add("firstName", user.FirstName);
                emailMessage.Parameters.Add("link", $"https://localhost:7284/users/change-password/{token}");
                _emailService.SendEmail(emailMessage);
            } else
            {
                throw new Exception("This email is not registered in our database");
            }
        }

        public async Task<UserEntity> GetByToken(string token, TokenType tokenType)
        {
            var user = await _userRepository.GetByToken(token, tokenType);
            return user;
        }

        public async Task<UserEntity> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return user;
        }

        public async Task VerifyEmail(int userID)
        {
            await _userRepository.VerifyEmail(userID);
        }
    }
}
