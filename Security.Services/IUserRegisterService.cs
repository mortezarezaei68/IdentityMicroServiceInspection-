using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Security.Service.ViewModel;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IUserRegisterService:IServiceApplication
    {
        Task<RegisterResponse> CreateAsyncUser(RegisterRequest userRegisterView);
        Task<object> LoginAsyncUser(UserLoginViewModel userRegisterView);
        Task<AuthenticationProperties> LoginFacebook();
        Task<object> LoginFacebookCallback();
        Task LogOut();
        Task<IdentityResult> UserEmailComfirmation(string userId, string token);
        Task<bool> ComfirmPhoneNumebrService(UserPhoneConfirmationViewModel PhoneNumber);
    }
}