using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Security.Services
{
    public class RequestResponseFactory:IRequestResponseFactory
    {
        private readonly Dictionary<Type, Type> _requestHandlerTypes;
        public RequestResponseFactory()
        {
            _requestHandlerTypes = typeof(RequestResponseFactory).Assembly.GetTypes()
                .Where(a => !a.IsAbstract).
                Select(t => new
                {
                    HandlerType = t,
                    RequestType = GetHandledRequestType(t)
                }).Where(x => x.RequestType != null)
                .ToDictionary(
                    x => x.RequestType,
                    x => x.HandlerType
                );
        }


        public static Type GetHandledRequestType(Type type)
        {
            var handlerInterface = type.GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IRequestHandler<,,>));

            return handlerInterface == null ? null : handlerInterface.GetGenericArguments()[0];
        }

        public async Task<TResponse> ProcessRequest<TRequest, TResponse,TViewModel>(TRequest request) 
            where TRequest : IRequestData<TResponse> where TResponse:ResponseResult<TViewModel>
        {
            var data = typeof(RequestResponseFactory).GetInterfaces();
            if (!_requestHandlerTypes.ContainsKey(typeof(TRequest)))
                throw new ApplicationException("No handler registered for type: " + typeof(TRequest).FullName);

            var handlerType = _requestHandlerTypes[typeof(TRequest)];

            var handler = (IRequestHandler<TRequest, TResponse,TViewModel>)Activator.CreateInstance(handlerType, BindingFlags.CreateInstance |
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.OptionalParamBinding, null, new object[] { Type.Missing }, CultureInfo.CurrentCulture,data);

            return await handler.ProcessRequest(request);
        }

        //public async Task<TResponse> ProcessRequest<TRequest, TResponse>(TRequest request)
        //    where TRequest : IRequestData<TResponse> 
        //{
        //    if (!_requestHandlerTypes.ContainsKey(typeof(TRequest)))
        //        throw new ApplicationException("No handler registered for type: " + typeof(TRequest).FullName);

        //    var handlerType = _requestHandlerTypes[typeof(TRequest)];

        //    var handler = (IRequestHandler<TRequest, TResponse>)Activator.CreateInstance(handlerType,BindingFlags.OptionalParamBinding);

        //    return await handler.ProcessRequest(request);
        //}
    }
}
