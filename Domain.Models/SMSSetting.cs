using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models
{
    public class SMSSetting
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LineNumber { get; set; }
        public bool CanContinueInCaseOfError { get; set; }
    }
}
