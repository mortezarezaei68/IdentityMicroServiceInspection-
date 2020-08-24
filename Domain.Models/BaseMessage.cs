using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models
{
    public class BaseMessage
    {
        public List<string> Messages { get; set; }
        public List<string> MobileNumbers { get; set; }
        public DateTime SendDateTime { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
