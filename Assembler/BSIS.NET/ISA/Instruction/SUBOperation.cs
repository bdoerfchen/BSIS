using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class SUBOperation : TargetedBasicOperation
    {
        public override byte GetOPCODE() => 0x13;

        public override List<string[]> GetExpectedTargets() => new()
        {
            new string[] {"A"},
            new string[] {"B"}
        };
    }
}
