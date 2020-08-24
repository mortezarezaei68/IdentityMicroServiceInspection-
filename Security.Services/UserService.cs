using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Security.Domain.Models;
using Security.Service.ViewModel;
using Security.Service.ViewModel.ResponseRequest.DataResponses.UserListRequestResponse;
using Security.View.ObjectMapper;

namespace Security.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserRegister> userManager;
        private readonly IMapper mapper;
        public UserService(UserManager<UserRegister> userManager,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public async Task<UserListResponse> GetAllUsers()
        {

            var userList = userManager.Users;
            var objmap=mapper.Map<IEnumerable<UserViewModel>>(userList);
            return new UserListResponse
            {
                ReturnObject = objmap
            };
        }
    }
}
