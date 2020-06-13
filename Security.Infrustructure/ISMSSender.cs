using Security.Domain.Models;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Infrustructure
{
    public interface ISMSSender
    {
        MessageResponse SendSMSAsync(List<string> number, List<string> message);
    }
}
