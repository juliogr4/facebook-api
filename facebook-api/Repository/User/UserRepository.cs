using Dapper;
using facebook_api.Database;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Utils;

namespace facebook_api.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _dapperContext;
        public UserRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;   
        }

        public async Task Create(UserEntity user)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_user_create;

                var parameters = new DynamicParameters();
                parameters.Add("first_name", user.FirstName);
                parameters.Add("last_name", user.LastName);
                parameters.Add("email", user.Email);
                parameters.Add("hash", user.Hash);
                parameters.Add("email_token", user.EmailToken);
                parameters.Add("date_of_birth", user.DOB);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task<UserEntity?> GetByEmail(string email)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_user_get_by_email;

                var parameters = new DynamicParameters();
                parameters.Add("email", email);

                var user = await conn.QuerySingleOrDefaultAsync<UserEntity>(procedure, parameters);
                return user;
            }
        }

        public async Task<UserEntity?> GetByToken(string token, TokenType tokenType)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_user_get_by_token;

                var parameters = new DynamicParameters();
                parameters.Add("token", token);
                parameters.Add("type", tokenType.ToString());

                var user = await conn.QuerySingleOrDefaultAsync<UserEntity>(procedure, parameters);
                return user;
            }
        }

        public async Task VerifyEmail(int userID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_user_verify_email;

                var parameters = new DynamicParameters();
                parameters.Add("user_id", userID);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }
    }
}
