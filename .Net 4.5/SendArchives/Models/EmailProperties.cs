using System.Net.Mail;

namespace SendArchives.Models
{
    public class EmailProperties
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public bool EnableSsl { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}