using facebook_api.Models.Enum;

namespace facebook_api.Models.DTO.Request.Reaction
{
    public class ReactionRequest
    {
        public int EntityID { get; set; }
        public int UserID { get; set; }
        public ReactionType Type { get; set; }
    }
}
