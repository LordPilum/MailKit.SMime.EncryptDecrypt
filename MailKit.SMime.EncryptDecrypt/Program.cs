using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.SMime.EncryptDecrypt.Common;
using MailKit.SMime.EncryptDecrypt.Sender;
using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Security;

namespace MailKit.SMime.EncryptDecrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);

                var collection = store.Certificates.Find(X509FindType.FindBySubjectName, "jan.kjetil.myklebust@hansencx.com", false); //TODO Change to true after test
                var senderCertificate = collection[0];

                /*store = new X509Store(StoreName.AddressBook, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);

                collection = store.Certificates.Find(X509FindType.FindBySubjectName, "jan.kjetil.myklebust@hansencx.com", false);*/ //TODO Change to true after test
                var recipientCertificate = collection[0];

                var mimeMessage = new MimeMessage
                {
                    Date = DateTime.Now,
                };

                mimeMessage.From.Add(
                    new SecureMailboxAddress(
                        "jan.kjetil.myklebust@hansencx.com",
                        "jan.kjetil.myklebust@hansencx.com",
                        senderCertificate.Thumbprint));

                mimeMessage.To.Add(
                    new SecureMailboxAddress(
                        "jan.kjetil.myklebust@hansencx.com",
                        "jan.kjetil.myklebust@hansencx.com",
                        recipientCertificate.Thumbprint));

                mimeMessage.Subject = "S/MIME Test";

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("TestAttachmentFile")))
                {
                    //Attachment
                    var attachment = new MimePart(new ContentType("text", "plain"))
                    {
                        ContentTransferEncoding =
                            ContentEncoding.Base64,
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        FileName = "TestAttachmentFileName.txt",
                        ContentObject = new ContentObject(stream)
                    };

                    var multipart = new Multipart("mixed") { attachment };

                    mimeMessage.Body = multipart;

                    //Sign / Encryption
                    var signer = new CmsSigner(senderCertificate);

                    var colle = new CmsRecipientCollection();
                    var bountyRecipientCertificate = DotNetUtilities.FromX509Certificate(recipientCertificate);

                    var recipient = new CmsRecipient(bountyRecipientCertificate);
                    colle.Add(recipient);

                    using (var ctx = new SecureSmimeContext())
                    {
                        var signed = MultipartSigned.Create(ctx, signer, mimeMessage.Body);
                        var encrypted = ApplicationPkcs7Mime.Encrypt(ctx, colle, signed);
                        mimeMessage.Body = MultipartSigned.Create(ctx, signer, encrypted);
                    }

                    //Sending
                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Connect("mail.hansencx.com", 465);
                        smtpClient.Send(mimeMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
