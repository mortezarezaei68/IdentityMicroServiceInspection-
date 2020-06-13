using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class UserLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
