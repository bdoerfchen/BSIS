using BSIS.NET.Exception;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class INOperation : IOperation
    {
        private const byte OPCODE_INA = 0x02;
        private const byte OPCODE_INB = 0x0D;

        public ushort GeneratesNumberOfBytes() => 1;

        public byte[] Parse(CommandLine line, DefinitionState definition)
        {
            if (line.Arguments.Count != 1)
                throw new InvalidInstructionArgumentNumberException(line, 1);

            var target = line.Arguments[0].ToUpper();
            if (target != "A" && target != "B")
                throw new InvalidInstructionArgumentException(line, 1, "Expecting target A or B");

            byte targetOpCode = target == "A" ? OPCODE_INA : OPCODE_INB;
            return new byte[] { targetOpCode };
        }
    }
}
