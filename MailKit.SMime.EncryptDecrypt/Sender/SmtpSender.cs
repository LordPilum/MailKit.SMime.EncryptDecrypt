using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailKit.SMime.EncryptDecrypt.Sender
{
    public class SmtpSender
    {
        public static async Task Send(MimeMessage message, SmtpProperties props)
        {
            var option = SecureSocketOptions.Auto;

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    // if there are errors in the certificate chain, look at each error to determine the cause.
                    if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == 0) return false;

                    if (chain?.ChainStatus != null)
                    {
                        foreach (var status in chain.ChainStatus)
                        {
                            if ((certificate.Subject == certificate.Issuer) && (status.Status == X509ChainStatusFlags.UntrustedRoot))
                            {
                                // self-signed certificates with an untrusted root are valid. 
                                continue;
                            }

                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // if there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }

                    // When processing reaches this line, the only errors in the certificate chain are 
                    // untrusted root errors for self-signed certificates. These certificates are valid
                    // for default Exchange server installations, so return true.
                    return true;

                };

                client.Connect(new Uri("foo"));//Connect(props.Host, props.SmtpPort, option);
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
