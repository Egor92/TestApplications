using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendArchives.Models
{
    public class EmailSender
    {
        private readonly EmailProperties _emailProperties;

        public EmailSender(EmailProperties emailProperties)
        {
            _emailProperties = emailProperties;
        }

        public Result Send(UserFileInfo archiveFileInfo)
        {
            try
            {
                using (var mail = new MailMessage(_emailProperties.SenderEmail, archiveFileInfo.Email))
                {
                    SmtpClient client = new SmtpClient
                    {
                        Host = _emailProperties.Host,
                        Port = _emailProperties.Port,
                        Timeout = _emailProperties.Timeout,
                        DeliveryMethod = _emailProperties.DeliveryMethod,
                        EnableSsl = _emailProperties.EnableSsl,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_emailProperties.UserName, _emailProperties.UserPassword, _emailProperties.Domain),
                    };

                    mail.BodyEncoding = Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mail.Attachments.Add(new Attachment(archiveFileInfo.FilePath));
                    mail.Subject = "Подключение к СИВ ОПОП";
                    mail.Body = _emailProperties.MessageBody;
                    client.Send(mail);
                }
                return new Result
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage = e.ToString(),
                };
            }
        }
    }
}
