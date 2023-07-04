using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Exception
{
    internal class InvalidInstructionArgumentException : InvalidInstructionException
    {
        const string MESSAGE = "Error at argument {0} of {1}: {2}";

        public InvalidInstructionArgumentException(CommandLine line, int argumentNr, string error)
            : base(line, string.Format(MESSAGE, line.Instruction, argumentNr, error))

        {

        }
    }
}
