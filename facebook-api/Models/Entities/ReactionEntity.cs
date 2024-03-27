using facebook_api.Models.Enum;

namespace facebook_api.Models.Entities
{
    public class ReactionEntity
    {
        public int EntityID { get; set; }
        public int UserID { get; set; }
        public ReactionType Type { get; set; }
    }
}
