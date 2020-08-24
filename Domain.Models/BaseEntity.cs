using System;

namespace Security.Domain.Models
{
    public class BaseEntity
    {
        public DateTime? CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public string UserCreation { get; set; }
    }
}
