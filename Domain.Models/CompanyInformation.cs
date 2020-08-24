using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models
{
    public class CompanyInformation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NationalId { get; set; }
        public string AddresseeName { get; set; }
        public string EconomicCode { get; set; }
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public virtual UserRegister User { get; set; }
    }
}
