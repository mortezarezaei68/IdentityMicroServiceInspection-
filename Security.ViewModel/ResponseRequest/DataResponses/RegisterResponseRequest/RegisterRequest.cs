using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class RegisterRequest: IRequestData<RegisterResponse>
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
    }
}
