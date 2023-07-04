using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.ISA
{
    internal class AssemblingError : IComparable<AssemblingError>
    {
        public AssemblingError(uint line, string message, ErrorCriticality criticality)
        {
            Line = line;
            Message = message;
            Criticality = criticality;
        }

        public uint Line { get; set; }
        public string Message { get; set; }
        public ErrorCriticality Criticality { get; set; }

        public int CompareTo(AssemblingError? other)
        {
            if (other is null) return -1;

            var critSort = Criticality - other.Criticality;
            if (critSort != 0)
                return critSort;
            else return (int)(Line - other.Line);
        }
    }

    internal enum ErrorCriticality { Info, Warning, Error, Critical }

    internal static class Errors
    {
        public const string ERROR_INVALIDPSEUDOARGUMENTS = "Invalid pseudo-command arguments. Expected: 2";
        public const string ERROR_INVALIDPSEUDO = "Invalid pseudo-command";
        public const string ERROR_INVALIDLABEL = "Invalid instruction label {0}. Label expects instruction in same line";

        public const string ERROR_PSEUDORESERVATIONEXISTS = "Reservation with label {0} already exists";
        public const string ERROR_PSEUDORESERVATIONLABELEXISTS = "Label {0} for reservation is already in use. Can not reserve memory";
        public const string ERROR_PSEUDOLABELEXISTS = "Label {0} already exists";
        public const string ERROR_PSEUDOCONSTANTEXISTS = "Constant {0} is already defined and will be overwritten";

        public const string ERROR_INSTRUCTIONNOTFOUND = "Instruction {0} is not valid";
        public const string ERROR_RESOLVEARGUMENT = "Can not resolve argument {0}";
        public const string ERROR_RESOLVECONSTANT = "Constant '{0}' is not declared";

        public const string ERROR_BUDGETEXCEEDED = "Program exceeds budget of {0} Bytes. Program takes {1} Bytes";

    }
}
