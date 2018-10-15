using EquipmentSupplyHandler.ConfigModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Notifications
{
    public class EmailNotificationSender : INotificationSender
    {
        readonly EmailConfig EmailSettings;
        public EmailNotificationSender(EmailConfig emailSettings)
        {
            EmailSettings = emailSettings;
        }
        public async Task SendAsync(Message message)
        {
            using (MailMessage mm = new MailMessage(EmailSettings.SMTPUser, EmailSettings.Email, message.Title, message.Body))
            {
                using (SmtpClient sc = new SmtpClient(EmailSettings.SMTPServer, EmailSettings.SMTPPort))
                {
                    sc.EnableSsl = true;
                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    sc.UseDefaultCredentials = false;
                    sc.Credentials = new NetworkCredential(EmailSettings.SMTPUser, EmailSettings.SMTPPassword);
                    await sc.SendMailAsync(mm);
                }
            }
        }
    }
}
