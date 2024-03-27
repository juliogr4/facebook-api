using facebook_api.Models.DTO.Request.Email;
using System.Net.Mail;

namespace facebook_api.Service.Email
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage email);
    }
}
