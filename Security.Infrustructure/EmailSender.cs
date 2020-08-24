using Microsoft.Extensions.Options;
using Security.Domain.Models;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Security.Infrustructure
{
    public class EmailSender : IEmailSender,IMessageSender
    {
        private readonly EmailSettingDomain _emailSettings;

        public EmailSender(EmailSettingDomain emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public MessageResponse SendEmailAsync(string email, string subject, List<string> messages)
        {
            string message="";
            try
            {
                foreach (var item in messages)
                {
                    message = $"{item}+{message}";
                }
                // Credentials
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = _emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSettings.MailServer,
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                return new MessageResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }

            return new MessageResponse
            {
                Message = "Email Send",
                Success = true
            };
        }


        public MessageResponse Send(BaseMessage baseMessage)
        {
            return SendEmailAsync(baseMessage.Email, baseMessage.Subject, baseMessage.Messages);
        }
    }
}
