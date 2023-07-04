using BSIS.NET.Exception;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class MOVOperation : IOperation
    {

        private const byte OPCODE_MOVAB = 0x10;
        private const byte OPCODE_MOVBA = 0x0B;

        public ushort GeneratesNumberOfBytes() => 1;


        public byte[] Parse(CommandLine line, DefinitionState definition)
        {
            if (line.Arguments.Count != 2)
                throw new InvalidInstructionArgumentNumberException(line, 2);

            var target = line.Arguments[0].ToUpper();
            if (target != "A" && target != "B")
                throw new InvalidInstructionArgumentException(line, 1, "Expecting target A or B");

            var source = line.Arguments[1].ToUpper();
            if (source != "A" && source != "B")
                throw new InvalidInstructionArgumentException(line, 1, "Expecting source A or B");

            byte targetOpCode = target == "A" ? OPCODE_MOVAB : OPCODE_MOVBA;
            return new byte[] { targetOpCode };
        }
    }
}
