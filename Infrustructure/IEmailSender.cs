using System.Threading.Tasks;

namespace Infrustructure
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

}