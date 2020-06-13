using Security.Domain.Models;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Infrustructure
{
    public interface IMessageSenderFactory
    {
        MessageResponse CreateMessage(BaseMessage baseMessage);
        bool IsValid(string emailaddress);
    }
}
