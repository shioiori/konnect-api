using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace Konnect.SMTP
{
    public class STMPHandler
    {
        private MailSettings _mailSettings;
        private SmtpClient _smtpClient;
        public STMPHandler()
        {
            _mailSettings = new MailSettings()
            {

            };
        }
        public STMPHandler(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }
        public void Connect(SecureSocketOptions secureSocketOptions = SecureSocketOptions.StartTls, CancellationToken cancellationToken = default)
        {
            try
            {
                _smtpClient.Connect(_mailSettings.Host, _mailSettings.Port, secureSocketOptions, cancellationToken);
                _smtpClient.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Disconnect(bool quit = true, CancellationToken cancellationToken = default)
        {
            _smtpClient.Disconnect(quit, cancellationToken);
        }

        public void Send(MimeMessage message)
        {
            AttactSenderInfo(message);
            SendInternal(message);
        }

        private int _retryTime = 0;
        private void SendInternal(MimeMessage message)
        {
            try
            {
                _smtpClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                if (_retryTime == 0) {
                    Connect();
                    SendInternal(message);
                    _retryTime++;
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                _retryTime = 0;
            }
        }

        private void AttactSenderInfo(MimeMessage message)
        {
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        }
    }
}
