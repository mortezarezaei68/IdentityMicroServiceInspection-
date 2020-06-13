using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class EmailRequest: IRequestData<MessageResponse>
    {
        public string EmailAddress { get; set; }
        public string Message { get; set; }
    }
}
