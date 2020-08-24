using Microsoft.Extensions.Options;
using Security.Domain.Models;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Security.Infrustructure
{
    public class MessageSenderFactory:IMessageSenderFactory
    {
        private readonly SMSSetting _smssettings;
        private readonly EmailSettingDomain _emailSetting;
        public MessageSenderFactory(IOptions<SMSSetting> smssettings,
            IOptions<EmailSettingDomain> emailsetting)
        {
            _smssettings=smssettings.Value;
            _emailSetting = emailsetting.Value;


        }
        public MessageResponse CreateMessage(BaseMessage baseMessage)
        {
            if (baseMessage.MobileNumbers != null)
            {
                return new SMSSender(_smssettings).Send(baseMessage);

            } else
            {
                return new EmailSender(_emailSetting).Send(baseMessage);
            }
        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
