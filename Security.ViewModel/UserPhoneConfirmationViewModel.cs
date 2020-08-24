using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class UserPhoneConfirmationViewModel
    {
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
    }
}
