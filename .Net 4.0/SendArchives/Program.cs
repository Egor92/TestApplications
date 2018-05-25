using System.Net.Mail;

namespace SendArchives
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var emailProperties = new EmailProperties
            {
                Host = "smtp.gmail.com",
                Port = 25,
                Timeout = 4000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                SenderEmail = "",
                SenderPassword = "",
            };

            var dataSender = new DataSender();
            dataSender.Send("input.txt", emailProperties);
        }
    }
}