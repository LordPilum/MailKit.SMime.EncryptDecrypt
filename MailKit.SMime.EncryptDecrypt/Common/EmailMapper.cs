using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MailKit.SMime.EncryptDecrypt.Exceptions;
using MimeKit;

namespace MailKit.SMime.EncryptDecrypt.Common
{
    public class EmailMapper
    {
        public static MimeMessage GetEdielMimeMessage(Email email)
        {
            var streamContent = email.Attachments.FirstOrDefault()?.Content;
            if (streamContent == null)
                throw new NoAttachmentException("No email attachments specified for Ediel message.");

            var message = new MimeMessage
            {
                Subject = email.Subject,
                Body = new MimePart(email.Attachments[0].MediaType, email.Attachments[0].MediaSubType)
                {
                    Content = new MimeContent(new MemoryStream(streamContent)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = email.Attachments[0].FileName
                }
            };

            foreach(var recipient in email.Recipients.FindAll(e => e.RecipientType.Equals(RecipientType.To)))
            {
                message.From.Add(new MailboxAddress(Encoding.ASCII, "", email.Sender.Address));
                message.To.Add(new MailboxAddress(Encoding.ASCII, recipient.FriendlyName, recipient.Address));
            }

            var contentType =
                new StringBuilder($@"{email.Attachments[0].MediaType}/{email.Attachments[0].MediaSubType}");
            if (email.Attachments[0].Charset != null)
            {
                contentType.AppendFormat(@"; Charset=""{0}""", email.Attachments[0].Charset);
            }

            message.Body.Headers["Content-Type"] = contentType.ToString();

            return message;
        }
    }
}
