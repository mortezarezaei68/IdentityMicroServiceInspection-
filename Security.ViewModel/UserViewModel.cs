using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string BirthDay { get; set; }
        public string RegType { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
