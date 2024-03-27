using facebook_api.Models.DTO.Request.Reaction;
using facebook_api.Models.Entities;

namespace facebook_api.Repository.Reaction
{
    public interface IReactionRepository
    {
        Task Create(ReactionEntity reactionEntity);
        Task Delete(int reactionID);
    }
}
