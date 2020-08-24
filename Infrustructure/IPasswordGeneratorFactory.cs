using System;
using System.Collections.Generic;
using System.Text;

namespace Infrustructure
{
    public interface IPasswordGeneratorFactory
    {
        IPasswordGenerator Create();
    }
}
