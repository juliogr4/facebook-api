using facebook_api.Models.DTO.Request.Reaction;

namespace facebook_api.Service.Reaction
{
    public interface IReactionService
    {
        Task Create(ReactionRequest reactionRequest);
        Task Delete(int reactionID);
    }
}
