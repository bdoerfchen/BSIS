using BSIS.NET.Exception;
using BSIS.NET.ISA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Model
{
    internal class CommandLine
    {
        public const char CONST_LITERAL = '#';

        public const string CONST_OCT_START = "0";
        public const string CONST_HEX_START = "0X";
        public const string CONST_BIN_START = "0B";

        public CommandLine(string line, uint sourceLine)
        {
            SourceLine = sourceLine;

            var commandLine = line.Split(Constants.COMMENT_LITERAL)[0].Trim();

            var tokens = commandLine.Split(ISA.Constants.ARGUMENT_SEPERATORS);
            if (tokens.Length == 0 || tokens[0].Length == 0)
            {
                IsEmpty = true;
                return;
            }
                
            Instruction = tokens[0].ToUpper();
            if (tokens.Length > 1)
                Arguments = tokens[1..].Where(t => t != string.Empty).ToList();

        }

        public string Instruction { get; set; } = string.Empty;
        public List<string> Arguments { get; set; } = new List<string>();
        public bool IsEmpty { get; set; }
        public uint SourceLine { get; set; }

        public static byte ParseByte(string argument, DefinitionState? definition = null)
        {
            if (argument.StartsWith(CONST_LITERAL))
            {
                //Resolve given constant value
                var literal = argument[1..].ToUpper();
                if (literal.StartsWith(CONST_HEX_START))
                {
                    return Convert.ToByte(literal[CONST_HEX_START.Length..], 16);
                } else if (literal.StartsWith(CONST_BIN_START))
                {
                    return Convert.ToByte(literal[CONST_HEX_START.Length..], 2);
                } else
                    return byte.Parse(literal, System.Globalization.NumberStyles.Number);
            } else if (definition is not null)
            {
                //Resolve label
                if (!definition.Constants.ContainsKey(argument.ToUpper()))
                {
                    throw new CouldNotParseArgumentException(string.Format(Errors.ERROR_RESOLVEARGUMENT, argument));
                }

                var value = definition.Constants[argument.ToUpper()];
                var resolvedValue = ParseByte(value, definition);
                return resolvedValue;

            } else
                throw new CouldNotParseArgumentException(string.Format(Errors.ERROR_RESOLVEARGUMENT, argument));
        }

        public override string ToString()
        {
            return $"{Instruction} {string.Join(" ", Arguments)}";
        }
    }
}
