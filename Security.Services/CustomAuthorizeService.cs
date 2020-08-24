using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Security.Domain.Models;
using Security.Service.ViewModel;

namespace Security.Services
{
    public class CustomAuthorizeService : ICustomAuthorizeService
    {
        private readonly IUserRoleService userRoleService;
        private readonly UserManager<UserRegister> userManager;
        public CustomAuthorizeService(IUserRoleService userRoleService,
            UserManager<UserRegister> userManager)
        {
            this.userRoleService = userRoleService;
            this.userManager = userManager;
        }
        public bool AuthorizedUser(AuthorizedViewModel authorizedViewModel)
        {
            var tokenS =DecodeToken(authorizedViewModel.Token);
            var data=userRoleService.GetAllUserRole(new UserViewModel
            {
                UserName = tokenS.Subject
            });
            var user = userManager.FindByNameAsync(tokenS.Subject);
            var currentUser = userManager.GetClaimsAsync(user.Result);
            if (currentUser.Result.Count == 0)
            {
                return false;
            }
            else if (Convert.ToDateTime(currentUser.Result.SingleOrDefault(a => a.Type == "exp").Value) < DateTime.Now)
            {
                 var removecliamsresult=userManager.RemoveClaimsAsync(user.Result, currentUser.Result).Result;
                if (removecliamsresult.Succeeded)
                {
                    return false;
                }
            }
            if (!CheckRoles(data.Result.Select(a => a.Role).ToList(),authorizedViewModel.Roles))
            {
                return false;
            }
            return true;
        }

        public JwtSecurityToken DecodeToken(string Token)
        {
            var stream = Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            return tokenS;
        }

        private bool CheckRoles(List<string> roles,List<string> userRoles)
        {
            foreach (var item in userRoles)
            {
                var roleResult=roles.SingleOrDefault(a => a == item);
                if (roleResult != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
