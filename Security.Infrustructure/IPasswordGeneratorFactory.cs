using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Infrustructure
{
    public interface IPasswordGeneratorFactory
    {
        IPasswordGenerator Create();
    }
}
