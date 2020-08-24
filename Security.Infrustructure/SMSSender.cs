using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Security.Domain.Models;
using Security.Service.ViewModel;
using SmsIrRestfulNetCore;

namespace Security.Infrustructure
{
    public class SMSSender : ISMSSender,IMessageSender
    {
        private readonly SMSSetting _smssettings;
        public SMSSender(SMSSetting smssettings)
        {
            _smssettings = smssettings;
        }

        public MessageResponse Send(BaseMessage baseMessage)
        {
            return SendSMSAsync(baseMessage.MobileNumbers, baseMessage.Messages);
        }

        public MessageResponse SendSMSAsync(List<string> number, List<string> message)
        {
            var token = new Token().GetToken("fcbdc9cbbf3b8f31ebb9ce4e", "asd!@#fg");

            var messageSendObject = new MessageSendObject()
            {
                Messages = message.ToArray(),
                MobileNumbers =number.ToArray(),
                LineNumber = _smssettings.LineNumber,
                SendDateTime = null,
                CanContinueInCaseOfError = _smssettings.CanContinueInCaseOfError
            };

            MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);
            return new MessageResponse
            {
                Message = messageSendResponseObject.Message,
                Success = messageSendResponseObject.IsSuccessful
            };
        }
    }
}
