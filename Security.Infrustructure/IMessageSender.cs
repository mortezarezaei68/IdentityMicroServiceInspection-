using Security.Domain.Models;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Infrustructure
{
    public interface IMessageSender
    {
        MessageResponse Send(BaseMessage baseMessage);
    }
}
