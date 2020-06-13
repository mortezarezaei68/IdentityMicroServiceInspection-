using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class UserRoleViewModel
    {
        public string RoleName { get; set; }
        public List<string> RolePermissions { get; set; }
    }
}
