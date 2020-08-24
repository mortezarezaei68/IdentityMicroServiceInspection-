using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Security.Services
{
    public interface ICustomAuthorizeService
    {
        bool AuthorizedUser(AuthorizedViewModel authorizedViewModel);
        JwtSecurityToken DecodeToken(string Token);
    }
}
