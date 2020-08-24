using Security.Service.ViewModel.ResponseRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Service.ViewModel
{
    public class ResponseResult<TObject>
    {
        public TObject ReturnObject { get; set; }
        public bool Success { get; set; }
        public List<MessageContent> responseMessage { get; set; }
    }
}
