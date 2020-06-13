using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Security.Domain.Models
{
    public class UserRegister : IdentityUser
    {
        public UserRegister()
        {
        }

        public UserRegister(string userName) : base(userName)
        {
        }
        public DateTime Birthday { get; set; }
        public string RegType { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public virtual CompanyInformation CompanyInformation { get; set; }
        public virtual PersonalInformation PersonalInformation { get; set; }
    }
}
