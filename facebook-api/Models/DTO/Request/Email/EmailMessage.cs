using System.Net.Mail;

namespace facebook_api.Models.DTO.Request.Email
{
    public class EmailMessage
    {
        public List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Body { get; set; }
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        public EmailMessage(List<string> to, string subject, string templateName, List<string>? Cc = null, List<string>? Bcc = null)
        {
            To = to;
            Subject = subject;
            TemplateName = templateName;
            this.Cc = Cc;
            this.Bcc = Bcc;
        }

        public MailMessage CreateEmailMessage()
        {
            var mailMessage = new MailMessage();

            foreach (var to in To)
            {
                mailMessage.To.Add(to);
            }

            if(Cc != null)
            {
                foreach (var cc in Cc)
                {
                    mailMessage.CC.Add(cc);
                }
            }

            if(Bcc != null)
            {
                foreach (var bcc in Bcc)
                {
                    mailMessage.Bcc.Add(bcc);
                }
            }

            if (!string.IsNullOrEmpty(TemplateName))
            {
                mailMessage.IsBodyHtml = true;

            }

            mailMessage.Subject = Subject;
            
            return mailMessage;
        }
    }
}
