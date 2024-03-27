namespace facebook_api.Models.DTO.Request.Email
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
    }
}
