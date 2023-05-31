using System.Net;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using SendGrid;

namespace Sendgrid.Email
{
    public class EmailService : IEmailService
    {
        private string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;
        }

        private SendGridMessage BuildMessage(MailAddress sender, IEnumerable<MailAddress> recipients)
        {
            var message = new SendGridMessage();

            message.SetFrom(new EmailAddress(sender.Address, sender.DisplayName));
            message.AddTos(recipients.Select(dest => new EmailAddress(dest.Address, dest.DisplayName)).ToList());

            return message;
        }

        public async Task SendEmailFromTemplate(MailAddress sender, IEnumerable<MailAddress> recipients, string templateId, ITemplateData templateData)
        {
            var message = this.BuildMessage(sender, recipients);

            message.SetTemplateId(templateId);
            message.SetTemplateData(templateData);

            await SendEmail(message);
        }

        public async Task SendEmailFromTemplate(MailAddress sender, IEnumerable<MailAddress> recipients, string templateId, object templateData)
        {
            var message = this.BuildMessage(sender, recipients);

            message.SetTemplateId(templateId);
            message.SetTemplateData(templateData);

            await SendEmail(message);
        }

        public async Task SendSimpleTextEmail(MailAddress sender, IEnumerable<MailAddress> recipients, string subject, string content)
        {
            var message = this.BuildMessage(sender, recipients);

            message.AddContent("text/plain", content);
            message.SetSubject(subject);

            await SendEmail(message);
        }

        public async Task SendEmail(SendGridMessage message)
        {
            var result = await new SendGridClient(_apiKey).SendEmailAsync(message);

            if (result.StatusCode != HttpStatusCode.Accepted)
            {
                var e = new Exception($"Email client replied {result.StatusCode} status code");
                e.Data["Body"] = result.Body.ReadAsStringAsync();
                throw e;
            }
        }
    }
}