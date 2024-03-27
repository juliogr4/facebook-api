using facebook_api.Models.DTO.Request.Email;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace facebook_api.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmailService(IOptions<EmailSettings> emailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _emailSettings = emailSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public void SendEmail(EmailMessage emailMessage)
        {
            try
            {
                var mailMessage = emailMessage.CreateEmailMessage();
                mailMessage.From = new MailAddress(_emailSettings.From);

                if(mailMessage.IsBodyHtml)
                {
                     mailMessage.Body = GetHTMLTemplate(emailMessage.TemplateName, emailMessage.Parameters);
                }
                SmtpClient smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port);

                smtp.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar o email", ex);
            }
        }

        public string GetHTMLTemplate(string htmlPath, Dictionary<string, object> parameters)
        {
            string webRoot = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRoot, "templates", htmlPath);
            string htmlContent = System.IO.File.ReadAllText(filePath);

            foreach (var parameter in parameters)
            {
                htmlContent = htmlContent.Replace($"{{{{ {parameter.Key} }}}}", parameter.Value.ToString());
            }

            return htmlContent;
        }
    }
}
