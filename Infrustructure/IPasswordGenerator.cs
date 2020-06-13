using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrustructure
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(PasswordOptions opts = null);
    }
}
