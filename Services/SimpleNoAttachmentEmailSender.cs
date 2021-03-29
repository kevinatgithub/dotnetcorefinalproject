using Microsoft.Extensions.Options;
using Services.DTO;
using System;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public class SimpleNoAttachmentEmailSender : IEmailSender
    {
        private SmtpConfig _smtpConfig;

        public SimpleNoAttachmentEmailSender(IOptions<SmtpConfig> options)
        {
            _smtpConfig = options.Value;
        }

        public void Send(EmailMessage model)
        {
            try
            {
                using (MailMessage mm = new MailMessage(_smtpConfig.Email, model.To))
                {
                    mm.Subject = model.Subject;
                    mm.Body = model.Body;

                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = _smtpConfig.Host;
                        smtp.EnableSsl = _smtpConfig.EnableSsl;
                        NetworkCredential NetworkCred = new NetworkCredential(_smtpConfig.Email, _smtpConfig.Password);
                        smtp.UseDefaultCredentials = _smtpConfig.UseDefaultCredentials;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = _smtpConfig.Port;
                        smtp.Send(mm);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
