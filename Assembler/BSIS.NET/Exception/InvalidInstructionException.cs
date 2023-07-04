using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Exception
{
    internal class InvalidInstructionException : ApplicationException
    {
        public CommandLine Line { get; set; }

        public InvalidInstructionException(CommandLine line, string Message) : base(Message)
        { 
            this.Line = line;
        }
    }
}
