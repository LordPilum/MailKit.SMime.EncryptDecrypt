namespace MailKit.SMime.EncryptDecrypt.Sender
{
    public class SmtpProperties
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int SmtpPort { get; set; }
        public string EmailAddress { get; set; }
        public bool UseAuth { get; set; }
        public bool UseSsl { get; set; }
    }
}
