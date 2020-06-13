using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class SMSRequest: IRequestData<MessageResponse>
    {
        public List<string> Number { get; set; }
    }
}
