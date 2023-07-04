using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal abstract class AddressedOperation : BasicOperation
    {
        public override ushort GeneratesNumberOfBytes() => 2;
        public override byte GetExpectedArgumentCount() => 1;
        public override byte[] Parse(CommandLine line, DefinitionState definition)
        {
            CheckArguments(line);

            byte addr = CommandLine.ParseByte(line.Arguments[0], definition);
            return new byte[] { GetOPCODE(), addr };
        }
    }
}
