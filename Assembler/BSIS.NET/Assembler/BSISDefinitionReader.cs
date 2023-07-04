using BSIS.NET.Exception;
using BSIS.NET.ISA;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Assembler
{
    internal class BSISDefinitionReader
    {
        public const char MARK_SEPARATOR = ':';

        public List<string> InstructionLines { get; set; } = new();
        public DefinitionState Definition { get; set; } = new();

        public List<AssemblingError> ErrorList { get; set; } = new();


        public BSISDefinitionReader(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();
                InstructionLines.AddRange(content.Split(Environment.NewLine));
            }
        }

        public DefinitionState ReadDefinition()
        {
            uint lineNr = 0;
            uint programLength = 0;
            foreach (var line in InstructionLines) 
            {
                lineNr++;

                //Ignore empty lines
                string trimmedLine = line.Trim();
                if (trimmedLine.Length == 0)
                    continue;

                CommandLine parsedLine = new CommandLine(trimmedLine, lineNr);
                if (parsedLine.IsEmpty)
                    continue;

                //Parse marked pseudo commands
                if (parsedLine.Instruction.EndsWith(MARK_SEPARATOR))
                {
                    var mark = parsedLine.Instruction[0..^1].ToUpper(); //Instruction without separator

                    //If no additional instruction is given, line is seen as label without instruction
                    if (parsedLine.Arguments.Count < 1)
                    {
                        AddError(
                            lineNr,
                            string.Format(Errors.ERROR_INVALIDLABEL, mark),
                            ErrorCriticality.Warning
                            );
                        continue;
                    }

                    var pseudoHelper = parsedLine.Arguments[0].ToUpper();
                    switch (pseudoHelper) 
                    {
                        // Define constant with value
                        case "EQU":
                            {

                                if (parsedLine.Arguments.Count < 2)
                                {
                                    AddError(lineNr, Errors.ERROR_INVALIDPSEUDOARGUMENTS);
                                    continue;
                                }

                                if (Definition.Constants.ContainsKey(mark))
                                {
                                    //If label already exists, overwrite it
                                    AddError(lineNr, string.Format(Errors.ERROR_PSEUDOCONSTANTEXISTS, mark), ErrorCriticality.Info);
                                    Definition.Constants[mark] = parsedLine.Arguments[1];
                                } else
                                {
                                    //If not exists, add definition
                                    Definition.Constants.Add(mark, parsedLine.Arguments[1]);
                                }
                            }
                            break;
                        // Define Byte (reserve 1 Byte with value)
                        case "DB":
                        // Reserve Byte (n Bytes, with value 0)
                        case "RESB":
                            {
                                //No data is given
                                if (parsedLine.Arguments.Count < 2)
                                {
                                    AddError(lineNr, Errors.ERROR_INVALIDPSEUDOARGUMENTS, ErrorCriticality.Error);
                                    continue;
                                }

                                //Other reservation uses same name
                                if (Definition.Reservations.ContainsKey(mark))
                                {
                                    AddError(lineNr, string.Format(Errors.ERROR_PSEUDORESERVATIONEXISTS, mark), ErrorCriticality.Warning);
                                    continue;
                                }

                                //Constant uses same name
                                if (Definition.Constants.ContainsKey(mark))
                                {
                                    AddError(lineNr, string.Format(Errors.ERROR_PSEUDORESERVATIONLABELEXISTS, mark), ErrorCriticality.Critical);
                                    continue;
                                }

                                //Argument either defines the value for DB or is the amount of bytes for RESB
                                byte parsedValue = 0;
                                try
                                {
                                    var value = parsedLine.Arguments[1];
                                    parsedValue = CommandLine.ParseByte(value);
                                } catch
                                {
                                    AddError(lineNr, string.Format(Errors.ERROR_RESOLVEARGUMENT, parsedLine.Arguments[1]), ErrorCriticality.Warning);
                                    continue;
                                }

                                if (pseudoHelper == "DB")
                                    Definition.Reservations.Add(mark, new MemoryReservation(1, parsedValue));
                                else 
                                    Definition.Reservations.Add(mark, new MemoryReservation(parsedValue));
                            }
                            break;
                        // Define label for line
                        default:
                            {
                                if (Definition.Constants.ContainsKey(mark))
                                {
                                    AddError(lineNr, string.Format(Errors.ERROR_PSEUDOLABELEXISTS, mark), ErrorCriticality.Warning);
                                    continue;
                                }

                                //Reread line without label
                                parsedLine = new CommandLine(trimmedLine.Substring(mark.Length + 1), lineNr);

                                if (!InstructionParser.IsValidOperation(parsedLine))
                                {
                                    AddError(lineNr,
                                        string.Format(Errors.ERROR_INSTRUCTIONNOTFOUND, parsedLine.Instruction),
                                        ErrorCriticality.Warning);
                                    continue;
                                }

                                //No pseudo-code, add line with line number
                                Definition.Instructions.Add(parsedLine);
                                Definition.Constants.Add(mark, "#" + programLength);
                                programLength += InstructionParser.GetOperation(parsedLine).GeneratesNumberOfBytes();
                            }
                            break;
                    }
                    continue;

                }

                //Else save line
                if (!InstructionParser.IsValidOperation(parsedLine))
                {
                    AddError(lineNr,
                        string.Format(Errors.ERROR_INSTRUCTIONNOTFOUND, parsedLine.Instruction),
                        ErrorCriticality.Warning);
                    continue;
                }
                programLength += InstructionParser.GetOperation(parsedLine).GeneratesNumberOfBytes();
                Definition.Instructions.Add(parsedLine);
            }


            //Generate data bytes definition after reading full code
            var reservations = Definition.Reservations.ToList();
            foreach (var reservation in reservations)
            {
                Definition.Constants.Add(reservation.Key, "#" + programLength);
                programLength += reservation.Value.AmountBytes;
            }
            Definition.ExpectedProgramLength = programLength;

            //Return entries
            return Definition;
        }

        private void AddError(uint line, string message, ErrorCriticality criticality = ErrorCriticality.Error)
        {
            ErrorList.Add(new AssemblingError(line, message, criticality));
        }
    }
}
