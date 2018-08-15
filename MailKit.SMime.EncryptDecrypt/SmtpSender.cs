using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailKit.SMime.EncryptDecrypt
{
    public class SmtpSender
    {

        private static async Task Send(MimeMessage message, SmtpProperties props)
        {
            var option = SecureSocketOptions.Auto;

            using (var client = new SmtpClient())
            {
                client.Connect(props.Host, props.SmtpPort, option);
                if (props.UseAuth)
                {
                    await client.AuthenticateAsync(props.UserName, props.Password);
                }

                await client.SendAsync(message);

                client.Disconnect(true);
            }
        }
    }
}
