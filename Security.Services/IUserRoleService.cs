using Microsoft.AspNetCore.Identity;
using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
   public interface IUserRoleService
    {
        Task<IdentityResult> CreateRoles(UserRoleViewModel userRoleView);
        Task<IEnumerable<RoleViewModel>> GetAllUserRole(UserViewModel userViewModel);
        Task<IdentityResult> AddUserRoles(UserRolesViewModel userRoleViewModel);
        Task<IdentityResult> DeleteUserRoles(UserRolesViewModel userRoleViewModel);
    }
}
