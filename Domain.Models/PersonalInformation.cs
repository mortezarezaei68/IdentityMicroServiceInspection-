using System;

namespace Security.Domain.Models
{
    public class PersonalInformation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NationalCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public virtual UserRegister User { get; set; }
    }
}