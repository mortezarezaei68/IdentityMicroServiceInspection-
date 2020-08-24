using Security.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface ICompanyRegisterService
    {
        Task<CompanyRegisterResponse> CreateAsyncCompany(CompanyRegisterRequest companyRegister);
    }
}
