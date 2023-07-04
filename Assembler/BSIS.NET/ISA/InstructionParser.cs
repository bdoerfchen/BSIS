using BSIS.NET.ISA.Instruction;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA
{
    internal static class InstructionParser
    {
        private static Dictionary<string, IOperation> Operations = new()
        {
            { "NOP", new NOPOperation() },
            { "ADD", new ADDOperation() },
            { "HALT", new HALTOperation() },
            { "INC", new INCOperation() },
            { "IN", new INOperation() },
            { "MOV", new MOVOperation() },
            { "OUT", new OUTOperation() },
            { "SET", new SETOperation() },
            { "SUB", new SUBOperation() },
            { "SW", new SWOperation() },
            { "LW", new LWOperation() },
            { "JMP", new JMPOperation() },
            { "BNZ", new BNZOperation() },
            { "OR", new OROperation() },
            { "AND", new ANDOperation() },
        };

        public static byte[] ParseLine(CommandLine line, DefinitionState definition)
        {
            return GetOperation(line).Parse(line, definition);
        }

        public static IOperation GetOperation(CommandLine line) => Operations[line.Instruction];

        public static bool IsValidOperation(CommandLine line) => Operations.ContainsKey(line.Instruction);
    }
}
