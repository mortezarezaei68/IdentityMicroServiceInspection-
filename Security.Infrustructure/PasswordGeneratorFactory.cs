using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Infrustructure
{
    public class PasswordGeneratorFactory : IPasswordGeneratorFactory
    {
        public IPasswordGenerator Create()
        {
            return new PasswordGenerator();
        }
    }
}
