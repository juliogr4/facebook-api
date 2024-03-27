namespace facebook_api.Models.Entities
{
    public class UserEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ImageName { get; set; }
        public DateTime DOB { get; set; }
        public string Hash { get; set; }
        public string EmailToken { get; set; }
        public DateTime EmailTokenExpiresAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public string PasswordToken { get; set; }
        public DateTime? PasswordTokenExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
