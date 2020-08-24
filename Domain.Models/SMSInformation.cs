using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models
{
    public class SMSInformation
    {
        public List<string> Messages { get; set; }
        public List<string> MobileNumbers { get; set; }
        public DateTime SendDateTime { get; set; }
    }
}
