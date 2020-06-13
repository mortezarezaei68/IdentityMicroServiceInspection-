using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, RegisterResponse,UserViewModel>
    {
        public async Task<RegisterResponse> ProcessRequest(RegisterRequest request)
        {
            return null;
        }
    }
}
