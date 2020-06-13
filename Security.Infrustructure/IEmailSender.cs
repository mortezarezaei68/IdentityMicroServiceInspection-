
using Security.Service.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Security.Infrustructure
{
    public interface IEmailSender
    {
        MessageResponse SendEmailAsync(string email, string subject, List<string> messages);
    }

}