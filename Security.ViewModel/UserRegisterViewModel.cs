using Microsoft.AspNetCore.Identity;
using System;

namespace Security.ViewModel
{
    public class UserRegisterViewModel
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }

    }
}
