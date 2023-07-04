using BSIS.NET.Exception;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal abstract class BasicOperation : IOperation
    {
        public abstract byte GetOPCODE();
        public virtual byte GetExpectedArgumentCount() => 0;

        public virtual ushort GeneratesNumberOfBytes() => 1;

        public virtual byte[] Parse(CommandLine line, DefinitionState definition)
        {
            CheckArguments(line);

            return new byte[] { GetOPCODE() };
        }

        public virtual void CheckArguments(CommandLine line)
        {
            int expectedArguments = GetExpectedArgumentCount();
            if (line.Arguments.Count != expectedArguments)
                throw new InvalidInstructionArgumentNumberException(line, expectedArguments);
        }
    }
}
