using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models
{
    public class MessageTokenHistory:BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Token { get; set; }

    }
}
