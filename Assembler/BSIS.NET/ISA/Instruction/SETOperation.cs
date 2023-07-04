using BSIS.NET.Exception;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class SETOperation : BasicOperation
    {
        public override byte GetOPCODE() => 0x07;
        public override ushort GeneratesNumberOfBytes() => 2;


        public override byte[] Parse(CommandLine line, DefinitionState definition)
        {
            if (line.Arguments.Count != 1)
                throw new InvalidInstructionArgumentNumberException(line, 1);

            int value = CommandLine.ParseByte(line.Arguments[0], definition);

            return new byte[] { GetOPCODE(), (byte)value };
        }
    }
}
