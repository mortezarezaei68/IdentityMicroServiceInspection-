using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Security.Domain.Models;
using Security.Infrustructure;
using Security.Service.ViewModel;
using Security.Service.ViewModel.ResponseRequest;
using Security.View.ObjectMapper;

namespace Security.Services
{
    public class CompanyRegisterService : ICompanyRegisterService
    {
        private readonly IHttpContextAccessor httpcontextaccessor;
        private readonly UserManager<UserRegister> userManager;
        private readonly SignInManager<UserRegister> signInUser;
        private readonly IPasswordGeneratorFactory passwordGeneratorFactory;
        private readonly IMessageSenderFactory messageSenderFactory;
        private readonly IRequestResponseFactory requestResponseFactory;
        private readonly IRegisterService registerService;
        private readonly IMapper mapper;
        public CompanyRegisterService(UserManager<UserRegister> userManager,
            SignInManager<UserRegister> signInUser,
            IPasswordGeneratorFactory passwordGeneratorFactory,
            IHttpContextAccessor httpcontextaccessor,
            IRequestResponseFactory requestResponseFactory,
            IMessageSenderFactory messageSenderFactory,
            IRegisterService registerService,
            IMapper mapper)
        {
            this.messageSenderFactory = messageSenderFactory;
            this.userManager = userManager;
            this.signInUser = signInUser;
            this.passwordGeneratorFactory = passwordGeneratorFactory;
            this.httpcontextaccessor = httpcontextaccessor;
            this.requestResponseFactory = requestResponseFactory;
            this.registerService = registerService;
            this.mapper = mapper;
        }
        public async Task<CompanyRegisterResponse> CreateAsyncCompany(CompanyRegisterRequest companyRegister)
        {
            string passwordvalue = passwordGeneratorFactory.Create().GeneratePassword();
            Random randomNumber = new Random();
            int r = randomNumber.Next(100000, 1000000);
            List<MessageContent> messageContent = new List<MessageContent>();
            bool IsValidEmail = messageSenderFactory.IsValid(companyRegister.CompanyRegister.Email);
            var user = new UserRegister { RegType="company",UserName = companyRegister.CompanyRegister.Email, Email = IsValidEmail == true ? companyRegister.CompanyRegister.Email : null, PhoneNumber = IsValidEmail == false ? companyRegister.CompanyRegister.Email : null, PasswordHash = passwordvalue, CompanyInformation = new CompanyInformation { AddresseeName = companyRegister.CompanyRegister.AddresseeName, Email = companyRegister.CompanyRegister.Email, EconomicCode = companyRegister.CompanyRegister.EconomicCode, NationalId = companyRegister.CompanyRegister.NationalId,  VerifyCode = r.ToString() } };
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
                        messages.Add($" {user.CompanyInformation.AddresseeName} Company <br>Please confirm your account by<br>your password:{passwordvalue}<br><br> <a href = '{HtmlEncoder.Default.Encode(querystring)}'> clicking here </a>.");
                        BaseMessage baseMessage = new BaseMessage
                        {
                            Email = user.Email,
                            Subject = "pleaseComfirmAccount",
                            Messages = messages

                        };
                        var data = messageSenderFactory.CreateMessage(baseMessage);
                        messageContent.Add(new MessageContent
                        {
                            Message = data.Message,
                            Code = null
                        });
                        return new CompanyRegisterResponse
                        {
                            ReturnObject = mapper.Map<CompanyRegisterViewModel>(user.CompanyInformation),
                            responseMessage = messageContent,
                            Success = true
                        };

                    }
                }
                messageContent.AddRange(result.Errors.Select(a => new MessageContent
                {
                    Code = a.Code,
                    Message = a.Description
                }));
                //var registerResponse = requestResponseFactory.ProcessRequest<RegisterRequest, RegisterResponse>(userRegisterView);
                return new CompanyRegisterResponse
                {
                    ReturnObject=mapper.Map<CompanyRegisterViewModel>(user.CompanyInformation),
                    responseMessage = messageContent,
                    Success = result.Succeeded
                };
            }
            catch (Exception ex)
            {

                return new CompanyRegisterResponse
                {
                    responseMessage = new List<MessageContent>
                    {
                        new MessageContent{
                            Message=ex.Message,
                            Code=null
                        }

                    }
                };
            }

        }
    }
}
