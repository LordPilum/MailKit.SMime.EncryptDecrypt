using System;

namespace MailKit.SMime.EncryptDecrypt.Exceptions
{
    [Serializable]
    public class NoAttachmentException : Exception
    {
        public NoAttachmentException() : base() { }
        public NoAttachmentException(string message) : base(message) { }
        public NoAttachmentException(string message, Exception inner) : base(message, inner) { }

        protected NoAttachmentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
