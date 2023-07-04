using BSIS.NET.Exception;
using BSIS.NET.ISA;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Assembler
{
    internal class BSISAssembler
    {
        Stream assembly;
        public BSISAssembler(Stream assembly)
        {
            this.assembly = assembly;
        }

        public AssembleResult Assemble(uint budget)
        {
            // -- FIRST PASS: Read definition with constants
            BSISDefinitionReader reader = new BSISDefinitionReader(assembly);
            var definition = reader.ReadDefinition();
            var errors = reader.ErrorList;

            //Exit, if there are errors
            if (errors.Where(e => e.Criticality >= ErrorCriticality.Error).Count() > 0)
                return new AssembleResult(null, errors);

            //Exit, if length budget is not met
            if (definition.ExpectedProgramLength > budget)
            {
                errors.Add(new AssemblingError(0, string.Format(Errors.ERROR_BUDGETEXCEEDED, budget, definition.ExpectedProgramLength), ErrorCriticality.Critical));
                return new AssembleResult(null, errors);
            }

            // -- SECOND PASS: Translate lines to byte code
            byte[] output = new byte[definition.ExpectedProgramLength];
            uint currentByte = 0;

            //Assemble code
            foreach (var line in definition.Instructions)
            {
                try
                {
                    var instructionBytes = InstructionParser.ParseLine(line, definition);
                    Array.Copy(instructionBytes, 0, output, currentByte, instructionBytes.Length);
                    currentByte += (uint)instructionBytes.Length;
                }
                catch (ArgumentException ex)
                {
                    //Instruction in line was not found
                    var d = ex.Data;
                    errors.Add(
                        new AssemblingError(
                            line.SourceLine,
                            string.Format(Errors.ERROR_INSTRUCTIONNOTFOUND, line.Instruction),
                            ErrorCriticality.Error)
                        );
                }
                catch (ApplicationException ex)
                {
                    //Syntactic or semantic error in line, raised by instruction
                    errors.Add(
                        new AssemblingError(
                            line.SourceLine,
                            ex.Message,
                            ErrorCriticality.Error)
                        );
                }
            }

            //Append data reservations
            var reservations = definition.Reservations.ToList();
            foreach (var reservation in reservations)
            {
                var bytes = new byte[reservation.Value.AmountBytes];
                if (reservation.Value.Value is not null)
                    Array.Fill(bytes, (byte)reservation.Value.Value);

                Array.Copy(bytes, 0, output, currentByte, bytes.Length);
                currentByte += (uint)bytes.Length;
            }


            var result = errors.Where(e => e.Criticality >= ErrorCriticality.Warning).Count() > 0 ? null : output;
            return new AssembleResult(result, errors);

        }

    }
}
