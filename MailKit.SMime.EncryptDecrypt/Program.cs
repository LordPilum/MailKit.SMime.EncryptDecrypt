using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailKit.SMime.EncryptDecrypt.Common;
using MailKit.SMime.EncryptDecrypt.Sender;
using MimeKit.Cryptography;

namespace MailKit.SMime.EncryptDecrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CryptographyContext.Register(typeof(SecureSmimeContext));

            var boot = new SecureSmimeContext();
            var sax = boot.EnabledEncryptionAlgorithms;

            var email = new Email
            {
                Subject = "Test",
                Sender = new EmailAddress
                {
                    Address = "jan.kjetil.myklebust@hansencx.com",
                    FriendlyName = "Jan Kjetil Myklebust",
                    RecipientType = RecipientType.Undefined
                },
                Recipients = new List<EmailAddress>
                {
                    new EmailAddress
                    {
                        Address = "jan.kjetil.myklebust@hansencx.com",
                        FriendlyName = "Jan Kjetil Myklebust",
                        RecipientType = RecipientType.To
                    }
                },
                Attachments = new List<MailAttachment>
                {
                    new MailAttachment
                    {
                        Charset = Encoding.UTF8.ToString(),
                        Content = Encoding.UTF8.GetBytes("Testmail"),
                        MediaType = "application",
                        MediaSubType = "EDIFACT"
                    }
                }
            };

            var msg = EmailMapper.GetEdielMimeMessage(email);
            var props = new SmtpProperties
            {
                EmailAddress = "jan.kjetil.myklebust@hansencx.com",
                Host = "hansencx.com"
            };

            SmtpSender.Send(msg, props).GetAwaiter();

            Console.WriteLine(msg.Attachments.FirstOrDefault()?.ToString());
            //Console.WriteLine(boot.GetPreferredEncryptionAlgorithm());
            Console.ReadKey();
        }
    }
}
