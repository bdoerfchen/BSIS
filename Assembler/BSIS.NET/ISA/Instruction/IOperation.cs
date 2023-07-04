using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal interface IOperation
    {
        public byte[] Parse(CommandLine line, DefinitionState definition);

        public ushort GeneratesNumberOfBytes();
    }
}
