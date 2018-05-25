using System.Net.Mail;

namespace SendArchives
{
    public class EmailProperties
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
    }
}