using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Exception
{
    internal class CouldNotParseArgumentException : ApplicationException
    {
        public CouldNotParseArgumentException(string? message) : base(message)
        {
        }
    }
}
