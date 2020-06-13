using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IRequestResponseFactory
    {
       Task<TResponse> ProcessRequest<TRequest, TResponse,TViewModel>(TRequest request) where TRequest : IRequestData<TResponse> where TResponse : ResponseResult<TViewModel>;
    }
}
