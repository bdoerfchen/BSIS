using BSIS.NET.Model;
using BSIS.NET.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA.Instruction
{
    internal abstract class TargetedBasicOperation : BasicOperation
    {
        public abstract List<string[]> GetExpectedTargets();

        public override byte GetExpectedArgumentCount() => (byte)GetExpectedTargets().Count;

        public override void CheckArguments(CommandLine line)
        {
            base.CheckArguments(line);

            var expectedTargets = GetExpectedTargets();
            for (int i = 0; i < expectedTargets.Count; i++)
            {
                string arg = line.Arguments[i].ToUpper();
                var target = expectedTargets[i].FirstOrDefault(t => t.ToUpper() == arg);
                if (target is null)
                {
                    throw new InvalidInstructionArgumentException(line, i + 1, "Target must one of {" + string.Join(", ", expectedTargets[i]) + "}");
                }
            }
        }
    }
}
