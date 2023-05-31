using System.Net.Mail;

namespace Sendgrid.Email
{
    public interface IEmailService
    {
        Task SendEmailFromTemplate(MailAddress sender, IEnumerable<MailAddress> recipients, string templateId, ITemplateData templateData);
        Task SendEmailFromTemplate(MailAddress sender, IEnumerable<MailAddress> recipients, string templateId, object templateData);
        Task SendSimpleTextEmail(MailAddress sender, IEnumerable<MailAddress> recipients, string subject, string message);
    }
}