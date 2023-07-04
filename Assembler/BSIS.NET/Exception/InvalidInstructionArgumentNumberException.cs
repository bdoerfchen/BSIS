using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Exception
{
    internal class InvalidInstructionArgumentNumberException : InvalidInstructionException
    {
        const string MESSAGE = "Instruction expects {0} arguments, but {1} were given";

        public InvalidInstructionArgumentNumberException(CommandLine line, int argumentsExcepted)
            : base(line, string.Format(MESSAGE, argumentsExcepted, line.Arguments.Count))
        {

        }
    }
}
