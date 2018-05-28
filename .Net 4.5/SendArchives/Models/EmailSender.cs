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
                using (var mail = new MailMessage(_emailProperties.UserName, archiveFileInfo.Email))
                {
                    var client = new SmtpClient
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
                    mail.Body = @"Здравствуйте!

Вам предоставлен доступ к Системе информационного взаимодействия общественных пунктов охраны порядка (СИВ ОПОП, Система).

Для работы с СИВ ОПОП в адресную строку браузера введите: https://opop.mos.ru 

Далее введите свой логин и пароль (в архиве).
При возникновении вопросов по работе с Системой обращайтесь в службу технической поддержки по E-mail: opop_support@mos.ru

Для получения пароля от архива с Вашими учетными данными обращайтесь по номеру: 

8 (495)988-22-70 доб. 77854";
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
