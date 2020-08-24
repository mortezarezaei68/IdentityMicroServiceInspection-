using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class ChangePassword
    {
        private string _ConfirmNewPassword;
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword {
            set
            {
                //if(value!=NewPassword || value==null)
                //    throw new NotSupportedException("ConfirmPassword Not Valid");
                _ConfirmNewPassword = value;
            }
            get
            {
                return _ConfirmNewPassword;
            }
        }
        public string Token { get; set; }
    }
}
