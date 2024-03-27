using Dapper;
using facebook_api.Database;
using facebook_api.Models.DTO.Request.Friendship;
using facebook_api.Models.Entities;
using facebook_api.Utils;

namespace facebook_api.Repository.Friendship
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DapperContext _dapperContext;

        public FriendshipRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<List<UserEntity>> GetByStatus(int userID, string status)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_friendship_get_by_status;

                var parameters = new DynamicParameters();
                parameters.Add("user_id", userID);
                parameters.Add("status", status);

                var users = await conn.QueryAsync<UserEntity>(procedure, parameters);
                return users.ToList();
            }
        }

        public async Task SendRequest(SendRequest sendRequest)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_friendship_send_request;

                var parameters = new DynamicParameters();
                parameters.Add("source_id", sendRequest.SourceID);
                parameters.Add("target_id", sendRequest.TargetID);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task UpdateRequest(UpdateRequest updateRequest)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_friendship_update_request;

                var parameters = new DynamicParameters();
                parameters.Add("source_id", updateRequest.SourceID);
                parameters.Add("target_id", updateRequest.TargetID);
                parameters.Add("status", updateRequest.Status);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }
    }
}
