using BSIS.NET.Exception;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal class ADDOperation : TargetedBasicOperation
    {
        public override byte GetOPCODE() => 0x0C;

        public override List<string[]> GetExpectedTargets() => new()
        {
            new string[] {"A", "B"},
            new string[] {"A", "B"}
        };
    }
}
