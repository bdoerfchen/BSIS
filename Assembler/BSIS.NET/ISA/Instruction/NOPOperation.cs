using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class NOPOperation : BasicOperation
    {
        public override byte GetOPCODE() => 0x01;
    }
}
