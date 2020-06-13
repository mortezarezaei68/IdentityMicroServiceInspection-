using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Security.Domain.Models;
using Security.Infrustructure;
using Security.Service.ViewModel;
using Security.Service.ViewModel.ResponseRequest;
using Security.Service.ViewModel.ResponseRequest.DataResponses.ChangePasswordRequestResponse;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public class RegisterService:IRegisterService
    {
        private readonly UserManager<UserRegister> userManager;
        private readonly IMessageSenderFactory messageSenderFactory;
        private readonly ICustomAuthorizeService customAuthorizeService;
        public RegisterService(UserManager<UserRegister> userManager,
                   IMessageSenderFactory messageSenderFactory,
                   ICustomAuthorizeService customAuthorizeService)
        {
            this.userManager = userManager;
            this.messageSenderFactory = messageSenderFactory;
            this.customAuthorizeService = customAuthorizeService;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePassword)
        {
            List<MessageContent> messageContents = new List<MessageContent>();
            try
            {
                //if (changePassword.ChangePassword.NewPassword != changePassword.ChangePassword.ConfirmNewPassword)
                //{
                //    throw new Exception("Not a valid ConfirmPassword");

                //}
                var tokenS = customAuthorizeService.DecodeToken(changePassword.ChangePassword.Token);
                var UserName = tokenS.Subject;
                var result = await userManager.FindByNameAsync(UserName);
                
                if (result!=null)
                {
                    var ConfirmOldPassword = await userManager.ChangePasswordAsync(result, changePassword.ChangePassword.OldPassword, changePassword.ChangePassword.NewPassword);
                    //var newpassword = userManager.PasswordHasher.HashPassword(result, changePassword.ChangePassword.NewPassword);
                    //result.PasswordHash = newpassword;
                    //var res = await userManager.UpdateAsync(result);
                    if (ConfirmOldPassword.Succeeded)
                    {
                        return new ChangePasswordResponse
                        {
                            responseMessage = null,
                            Success = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                messageContents.Add(
                new MessageContent
                {
                    Code=HttpStatusCode.BadRequest.ToString(),
                    Message= ex.Message
                }
            );
            }
 
            return new ChangePasswordResponse
            {
                responseMessage =messageContents,
                Success = false
            };

        }

        public async Task<IdentityResult> ComfirmEmailService(string userId, string token)
        {
            var user = await userManager.FindByNameAsync(userId);
            var result = await userManager.ConfirmEmailAsync(user, token);
            return result;
        }
        public async Task<bool> ComfirmPhoneNumebrService(UserPhoneConfirmationViewModel PhoneNumber)
        {
            bool confirmationCode;
            var result = await userManager.FindByNameAsync(PhoneNumber.PhoneNumber);
            if (PhoneNumber.Type == "user")
            {
                 confirmationCode = result.PersonalInformation.VerifyCode == PhoneNumber.Code ? true : false;
            }
            else
            {
                 confirmationCode = result.CompanyInformation.VerifyCode == PhoneNumber.Code ? true : false;
            }
            var confirmation = await userManager.IsPhoneNumberConfirmedAsync(result);
            if (confirmation && confirmationCode)
            {
                result.PhoneNumberConfirmed = true;
                await userManager.UpdateAsync(result);
                return true;
            }
            return false;
        }
        /// <summary>
        /// generate token for user
        /// </summary>
        /// <param name="result">user data retrieve from database</param>
        /// <returns>token and expiration date</returns>
        public async Task<object> generateToken(UserRegister result)
        {
            var roles = await userManager.GetRolesAsync(result);
            DateTime currentDate = DateTime.Now.AddHours(1);
            List<Claim> claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Sub,result.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp,currentDate.ToString())
                };
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));
            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                expires: currentDate,
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            await userManager.AddClaimsAsync(result, claims);
            return results;
        }
    }
}
