using System.Collections.Generic;

namespace MailKit.SMime.EncryptDecrypt.Common
{
    public class Email
    {
        public Email()
        {
            Recipients = new List<EmailAddress>();
            Attachments = new List<MailAttachment>();
        }

        public string Subject { get; set; }
        public EmailAddress Sender { get; set; }
        public List<EmailAddress> Recipients { get; set; }
        public string PlainTextBody { get; set; }
        public string HtmlBody { get; set; }
        public List<MailAttachment> Attachments { get; set; }

    }
}
