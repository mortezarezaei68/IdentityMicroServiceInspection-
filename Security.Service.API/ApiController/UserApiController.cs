using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Service.ViewModel;
using Security.Service.ViewModel.ResponseRequest.DataResponses.ChangePasswordRequestResponse;
using Security.Service.ViewModel.ResponseRequest.DataResponses.UserListRequestResponse;
using Security.Services;

namespace Security.Service.API.ApiController
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserRegisterService userRegisterService;
        private readonly ICompanyRegisterService companyRegisterService;
        private readonly IUserRoleService userRoleService;
        private readonly IRequestResponseFactory requestResponseFactory;
        private readonly ICustomAuthorizeService customAuthorizeService;
        private readonly IUserService userService;
        private readonly IRegisterService registerService;
        public UserApiController(IUserRegisterService userRegisterService,
            IUserRoleService userRoleService,
            IUserService userService,
            IRequestResponseFactory requestResponseFactory,
            ICustomAuthorizeService customAuthorizeService,
            ICompanyRegisterService companyRegisterService,
            IRegisterService registerService)
        {
            this.requestResponseFactory = requestResponseFactory;
            this.userRegisterService = userRegisterService;
            this.userRoleService = userRoleService;
            this.customAuthorizeService = customAuthorizeService;
            this.userService = userService;
            this.companyRegisterService = companyRegisterService;
            this.registerService = registerService;
        }
        [HttpPost]
        [Route("UserCreate")]
        public async Task<RegisterResponse> CreateUser([FromBody]RegisterRequest userRegisterView)
        {
            //var result = await requestResponseFactory.ProcessRequest<RegisterRequest, RegisterResponse>(userRegisterView);
            var result = await userRegisterService.CreateAsyncUser(userRegisterView);
            return result;
        }
        [HttpPost]
        [Route("CompanyCreate")]
        public async Task<CompanyRegisterResponse> CreateCompany([FromBody]CompanyRegisterRequest companyRegister)
        {
            var result = await companyRegisterService.CreateAsyncCompany(companyRegister);
            return result;
        }
        [HttpPost]
        [Route("UserLogin")]
        public async Task<object> LoginUser([FromBody]UserLoginViewModel userLoginView)
        {
            var result=await userRegisterService.LoginAsyncUser(userLoginView);
            return result;
        }
        [HttpGet]
        [Route("facebookCheckLogin")]
        public async Task<IActionResult> LoginFacebook()
        {
             var properties=await userRegisterService.LoginFacebook();
            return new ChallengeResult("Facebook",properties);
        }
        [HttpGet]
        [Route("facebookLogin")]
        public async Task<object> LoginWithFacebook()
        {
            return await userRegisterService.LoginFacebookCallback();
        }
        [HttpGet]
        [Route("signOut")]
        public async Task SignOut()
        {
            await userRegisterService.LogOut();
        }
        [HttpPost]
        [Route("comfirmedPhoneNumber")]
        public async Task<object> UserPhoneNumber([FromBody]UserPhoneConfirmationViewModel userLoginView)
        {
            var result = await userRegisterService.ComfirmPhoneNumebrService(userLoginView);
            return result;
        }
        [HttpGet]
        [Route("comfirmedEmail")]
        public async Task<bool> comfirmedEmail( [FromQuery] string accepted,[FromQuery] string userId)
        {
            var result=await userRegisterService.UserEmailComfirmation(userId, accepted);
            return result.Succeeded;
        }
        [HttpPost]
        [Route("roleCreate")]
        public async Task<bool> RoleCreate([FromBody]UserRoleViewModel userRoleView)
        {
            var result= await userRoleService.CreateRoles(userRoleView);
            return result.Succeeded;
        }
        [HttpPost]
        [Route("roleAssign")]
        public async Task<bool> roleAssignToUser([FromBody]UserRolesViewModel userRolesView)
        {
            var result = await userRoleService.AddUserRoles(userRolesView);
            return result.Succeeded;
        }
        [HttpPost]
        [Route("getUserRole")]
        public async Task<IEnumerable<RoleViewModel>> GetUserRoles([FromBody]UserViewModel userViewModel)
        {
            return await userRoleService.GetAllUserRole(userViewModel);
        }
        [HttpPost]
        [Route("DeleteUserRole")]
        public async Task<bool> DeleteUserRoles([FromBody]UserRolesViewModel userViewModel)
        {
            var result=await userRoleService.DeleteUserRoles(userViewModel);
            return result.Succeeded;
        }
        [HttpPost]
        [Route("checkUserAuhtorize")]
        public bool DeleteUserRoles([FromBody]AuthorizedViewModel authorizedView)
        {
            var result = customAuthorizeService.AuthorizedUser(authorizedView);
            return result;
        }
        [HttpGet]
        [Route("getAllUsers")]
        public async Task<UserListResponse> getAllUsers()
        {
            return await userService.GetAllUsers();
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ChangePasswordResponse> ChangePassword([FromBody]ChangePasswordRequest changePassword)
        {
            changePassword.ChangePassword.Token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return await registerService.ChangePassword(changePassword);
        }
    }
}