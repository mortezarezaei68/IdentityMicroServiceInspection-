using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel
{
    public class CompanyRegisterRequest: IRequestData<CompanyRegisterResponse>
    {
        public CompanyRegisterViewModel CompanyRegister { get; set; }
    }
}
