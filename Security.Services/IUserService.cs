using Security.Service.ViewModel.ResponseRequest.DataResponses.UserListRequestResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IUserService
    {
        Task<UserListResponse> GetAllUsers();
    }
}
