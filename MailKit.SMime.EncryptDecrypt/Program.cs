using System;
using MailKit.SMime.EncryptDecrypt.Common;
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
            Console.WriteLine(sax.Length);
        }
    }
}
