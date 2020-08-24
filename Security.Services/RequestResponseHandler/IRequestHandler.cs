using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services
{
   public interface IRequestHandler<TRequest,TResponse,TViewModel> where TResponse:ResponseResult<TViewModel>
    {
        Task<TResponse> ProcessRequest(TRequest request);
    }
}
