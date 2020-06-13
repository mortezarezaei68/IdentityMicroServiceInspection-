using Microsoft.AspNetCore.Identity;
using Security.Domain.Models;
using Security.Service.ViewModel;
using Security.Service.ViewModel.ResponseRequest;
using Security.Service.ViewModel.ResponseRequest.DataResponses.ChangePasswordRequestResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IRegisterService
    {
        Task<IdentityResult> ComfirmEmailService(string userId, string token);
        Task<bool> ComfirmPhoneNumebrService(UserPhoneConfirmationViewModel PhoneNumber);
        Task<object> generateToken(UserRegister result);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePassword);
    }
}
