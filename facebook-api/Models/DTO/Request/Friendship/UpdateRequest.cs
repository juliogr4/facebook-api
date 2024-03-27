using facebook_api.Models.Enum;

namespace facebook_api.Models.DTO.Request.Friendship
{
    public class UpdateRequest
    {
        public int SourceID { get; set; }
        public int TargetID { get; set; }
        public string Status { get; set; }
    }
}
