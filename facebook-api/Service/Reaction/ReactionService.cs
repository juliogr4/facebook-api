using facebook_api.Models.DTO.Request.Reaction;
using facebook_api.Models.Entities;
using facebook_api.Repository.Reaction;

namespace facebook_api.Service.Reaction
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        public ReactionService(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task Create(ReactionRequest reactionRequest)
        {
            var reaction = new ReactionEntity
            {
                EntityID = reactionRequest.EntityID,
                Type = reactionRequest.Type,
                UserID = reactionRequest.UserID
            };
            await _reactionRepository.Create(reaction);
        }

        public async Task Delete(int reactionID)
        {
            await _reactionRepository.Delete(reactionID);
        }
    }
}
