using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Infrustructure
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(PasswordOptions opts = null);
    }
}
