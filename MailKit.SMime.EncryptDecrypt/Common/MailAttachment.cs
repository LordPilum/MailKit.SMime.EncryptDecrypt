using System;
using System.Collections.Generic;
using System.Text;

namespace MailKit.SMime.EncryptDecrypt.Common
{
    public class MailAttachment
    {
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }
        public string Charset { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
