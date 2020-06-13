using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Security.Domain.Models;
using Security.Service.ViewModel;
using Security.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Security.Infrustructure;
using Security.Service.ViewModel.ResponseRequest;
using AutoMapper;
using Security.View.ObjectMapper;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Authentication;

namespace Security.Services
{
    public class UserRegisterService : IUserRegisterService
    {
        private readonly IHttpContextAccessor httpcontextaccessor;
        private readonly UserManager<UserRegister> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly SignInManager<UserRegister> signInUser;
        private readonly IPasswordGeneratorFactory passwordGeneratorFactory;
        private readonly IMessageSenderFactory messageSenderFactory;
        private readonly IRequestResponseFactory requestResponseFactory;
        private readonly IRegisterService registerService;
        private readonly IMapper mapper;
        private readonly FacebookSetting facebookSetting;
        public UserRegisterService(UserManager<UserRegister> userManager,
            SignInManager<UserRegister> signInUser,
            IMapper mapper,
            RoleManager<UserRole> roleManager,
            IPasswordGeneratorFactory passwordGeneratorFactory,
            IHttpContextAccessor httpcontextaccessor,
            IRequestResponseFactory requestResponseFactory,
            IMessageSenderFactory messageSenderFactory,
            IRegisterService registerService,
            IOptions<FacebookSetting> facebookSetting)
        {
            this.messageSenderFactory = messageSenderFactory;
            this.userManager = userManager;
            this.signInUser = signInUser;
            this.roleManager = roleManager;
            this.passwordGeneratorFactory = passwordGeneratorFactory;
            this.httpcontextaccessor = httpcontextaccessor;
            this.requestResponseFactory = requestResponseFactory;
            this.registerService = registerService;
            this.mapper = mapper;
            this.facebookSetting = facebookSetting.Value;
        }
        public async Task<RegisterResponse> CreateAsyncUser(RegisterRequest userRegisterView)
        {
            string passwordvalue = passwordGeneratorFactory.Create().GeneratePassword();
            Random randomNumber = new Random();
            int r = randomNumber.Next(100000, 1000000);
            List<MessageContent> messageContent = new List<MessageContent>();
            bool IsValidEmail=messageSenderFactory.IsValid(userRegisterView.EmailAddress);
            var user = new UserRegister { RegType="user",UserName = userRegisterView.EmailAddress, Email = IsValidEmail==true ? userRegisterView.EmailAddress : null,PhoneNumber= IsValidEmail == false ? userRegisterView.EmailAddress : null, PasswordHash = passwordvalue, PersonalInformation = new PersonalInformation { UserName= userRegisterView.EmailAddress,Email=userRegisterView.EmailAddress,NationalCode=userRegisterView.NationalCode,FirstName=userRegisterView.FirstName,LastName=userRegisterView.LastName,VerifyCode=r.ToString() } };
            try
            {
                List<string> messages = new List<string>();

                var result = await userManager.CreateAsync(user, passwordvalue);
                if (result.Succeeded)
                {
                    if (IsValidEmail)
                    {
                        string ctoken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var request = httpcontextaccessor.HttpContext.Request;
                        var queryparams = new Dictionary<string, string>
                                        {
                                            {"accepted", ctoken},
                                            {"userId",user.UserName }
                                        };
                        var querystring = QueryHelpers.AddQueryString("https://localhost:44326/api/UserAuth/comfirmedEmail", queryparams);
                        messages.Add($"Dear {user.PersonalInformation.FirstName}' '{user.PersonalInformation.LastName}<br>Please confirm your account by<br>your password:{passwordvalue}<br><br> <a href = '{HtmlEncoder.Default.Encode(querystring)}'> clicking here </a>.");
                        BaseMessage baseMessage = new BaseMessage
                        {
                            Email = user.Email,
                            Subject = "pleaseComfirmAccount",
                            Messages = messages

                        };
                        var data=messageSenderFactory.CreateMessage(baseMessage);
                        messageContent.Add(new MessageContent
                        {
                            Message = data.Message,
                            Code = null
                        });
                        return new RegisterResponse
                        {
                            ReturnObject= mapper.Map<UserViewModel>(user.PersonalInformation),
                            responseMessage = messageContent,
                            Success = data.Success
                        };

                    }
                    else
                    {
                        messages.Clear();
                        List<string> numbers = new List<string>();
                        numbers.Add(user.PhoneNumber);
                        messages.Add($"your activationcode :{r.ToString()} and your password:{passwordvalue}");
                        var data=messageSenderFactory.CreateMessage(new BaseMessage {
                            Messages=messages,
                            MobileNumbers=numbers
                        });
                        messageContent.Add(new MessageContent
                        {
                            Message = data.Message,
                            Code = null
                        });
                        return new RegisterResponse
                        {
                            ReturnObject = mapper.Map<UserViewModel>(user.PersonalInformation),
                            responseMessage = messageContent,
                            Success = data.Success
                        };
                    }
                }
                messageContent.AddRange(result.Errors.Select(a => new MessageContent
                {
                    Code = a.Code,
                    Message = a.Description
                }));
                //var registerResponse = requestResponseFactory.ProcessRequest<RegisterRequest, RegisterResponse>(userRegisterView);
                return new RegisterResponse {
                    ReturnObject = mapper.Map<UserViewModel>(user.PersonalInformation),
                    responseMessage= messageContent,
                    Success=result.Succeeded
                };
            }
            catch (Exception ex)
            {

                return new RegisterResponse {
                    responseMessage=new List<MessageContent>
                    {
                        new MessageContent{
                            Message=ex.Message,
                            Code=null
                        }

                    }
                };
            }

        }


        public async Task<object> LoginAsyncUser(UserLoginViewModel userLoginView)
        {
            var result=await userManager.FindByNameAsync(userLoginView.UserName);
            var passwordChecker = await signInUser.CheckPasswordSignInAsync(result, userLoginView.Password, false);
            if (result!=null && passwordChecker.Succeeded)
            {
                var currentUser =await userManager.GetClaimsAsync(result);
                if (currentUser.Count == 0)
                {
                    return await registerService.generateToken(result);
                }else if (currentUser.Count!=0 || Convert.ToDateTime(currentUser.SingleOrDefault(a => a.Type == "exp").Value) > DateTime.Now)
                {
                    await userManager.RemoveClaimsAsync(result, currentUser);
                    return await registerService.generateToken(result);
                }

            }
            return false;
        }

        public async Task<IdentityResult> UserEmailComfirmation(string userId, string token)
        {
            return await registerService.ComfirmEmailService(userId, token);
        }

        public async Task<bool> ComfirmPhoneNumebrService(UserPhoneConfirmationViewModel PhoneNumber)
        {
            PhoneNumber.Type = "user";
            return await registerService.ComfirmPhoneNumebrService(PhoneNumber);
        }

        public async Task<AuthenticationProperties> LoginFacebook()
        {
            var properties=signInUser.ConfigureExternalAuthenticationProperties("Facebook", "/api/UserAuth/facebookLogin");

            //HttpClient httpClient = new HttpClient();
            //var queryparams = new Dictionary<string, string>
            //                            {
            //                                {"client_id",facebookSetting.AppId },
            //                                {"display","popup" },
            //                                {"response_type","token"},
            //                                {"redirect_uri","https%3A%2F%2Flocalhost%3A44326%2Fapi%2FUserAuth%2FfacebookLogin"}
            //                            };

            //var querystring = QueryHelpers.AddQueryString("https://www.facebook.com/dialog/oauth", queryparams);
            ////var result = await httpClient.GetAsync(HtmlEncoder.Default.Encode(querystring));
            //var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            //response.Headers.Location = new Uri(querystring);

            return properties;
        }

        public async Task<object> LoginFacebookCallback()
        {
            try
            {
                var info = await signInUser.GetExternalLoginInfoAsync();
                if (info == null)
                    throw new Exception($"you not login to facebook");
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new UserRegister
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInUser.SignInAsync(user, isPersistent: false);
                    return info.AuthenticationTokens;
                }
                throw new Exception("you have error");
            }
            catch (Exception Message)
            {
                return Message;
            }

        }

        public async Task LogOut()
        {
            await signInUser.SignOutAsync();
        }
    }
}
