using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class AuthorizedViewModel
    {
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
