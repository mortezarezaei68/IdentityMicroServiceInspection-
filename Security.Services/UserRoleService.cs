using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Security.Domain.Models;
using Security.Service.ViewModel;
using Security.ViewModel;

namespace Security.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly RoleManager<UserRole> roleManager;
        private readonly UserManager<UserRegister> userManager;
        public UserRoleService(RoleManager<UserRole> roleManager,
                                UserManager<UserRegister> userManager) 
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IdentityResult> CreateRoles(UserRoleViewModel userRoleView)
        {
            var roleExist=await roleManager.FindByNameAsync(userRoleView.RoleName);
            if (roleExist == null)
            {
                var result = await roleManager.CreateAsync(new UserRole() { Name = userRoleView.RoleName });
                if (result.Succeeded)
                {
                    var roleresult = await roleManager.FindByNameAsync(userRoleView.RoleName);
                    foreach (var item in userRoleView.RolePermissions)
                    {
                        await roleManager.AddClaimAsync(roleresult, new Claim(userRoleView.RoleName, item));
                    }
                    return result;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllUserRole(UserViewModel userViewModel)
        {
            var user = await userManager.FindByNameAsync(userViewModel.UserName);
            var roles=await userManager.GetRolesAsync(user);
            List<RoleViewModel> roleViewModels = new List<RoleViewModel>(); ;
            foreach (var item in roles)
            {
                roleViewModels.Add(new RoleViewModel
                {
                    Role = item
                });
            }
            return roleViewModels;
        }
        public async Task<IdentityResult> AddUserRoles(UserRolesViewModel userRoleViewModel)
        {
            var result = await userManager.FindByNameAsync(userRoleViewModel.UserName);
            var roleresult = await roleManager.GetRoleNameAsync(new UserRole() { Name = userRoleViewModel.RoleName });
            if (result != null && roleresult != null)
            {
                var results = await userManager.AddToRoleAsync(result, roleresult);
                return results;
            }
            return null;
        }
        public async Task<IdentityResult> DeleteUserRoles(UserRolesViewModel userRoleViewModel)
        {
            var user = await userManager.FindByNameAsync(userRoleViewModel.UserName);
            var roles = await userManager.GetRolesAsync(user);
            var roleExist = roles.SingleOrDefault(a => a == userRoleViewModel.RoleName);
            if (roleExist != null)
            {
                var result = await userManager.RemoveFromRoleAsync(user, roleExist);
                return result;
            }
            return null;

        }
    }
}
