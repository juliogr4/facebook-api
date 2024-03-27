using Dapper;
using facebook_api.Database;
using facebook_api.Models.DTO.Request.Reaction;
using facebook_api.Models.Entities;
using facebook_api.Utils;

namespace facebook_api.Repository.Reaction
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly DapperContext _dapperContext;
        public ReactionRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task Create(ReactionEntity reactionEntity)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_reaction_like;

                var parameters = new DynamicParameters();
                parameters.Add("entity_id", reactionEntity.EntityID);
                parameters.Add("user_id", reactionEntity.UserID);
                parameters.Add("type", reactionEntity.Type.ToString());

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task Delete(int reactionID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_reaction_dislike;

                var parameters = new DynamicParameters();
                parameters.Add("reaction_id", reactionID);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }
    }
}
