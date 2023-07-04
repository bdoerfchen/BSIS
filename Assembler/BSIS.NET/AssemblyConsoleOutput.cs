using BSIS.NET.ISA;
using BSIS.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET
{
    internal static class AssemblyConsoleOutput
    {
        public static void Output(AssembleResult result)
        {
            PrintErrors(result.Errors);
            if (!result.HasFailed())
                PrintCode(result.ByteCode!);
        }


        static void PrintErrors(List<AssemblingError> errors)
        {
            var startingForeground = Console.ForegroundColor;

            var numberInfos = errors.Count(e => e.Criticality == ErrorCriticality.Info);
            var numberWarnings = errors.Count(e => e.Criticality == ErrorCriticality.Warning);
            var numberErrors = errors.Count(e => e.Criticality == ErrorCriticality.Error);
            var numberCriticals = errors.Count(e => e.Criticality == ErrorCriticality.Critical);

            Console.WriteLine("Assembly Feedback:");
            Console.ForegroundColor = ColorOfCriticality(ErrorCriticality.Info);
            Console.WriteLine($"\t{numberInfos} Infos");
            Console.ForegroundColor = ColorOfCriticality(ErrorCriticality.Warning);
            Console.WriteLine($"\t{numberWarnings} Warnings");
            Console.ForegroundColor = ColorOfCriticality(ErrorCriticality.Error);
            Console.WriteLine($"\t{numberErrors} Errors");
            Console.ForegroundColor = ColorOfCriticality(ErrorCriticality.Critical);
            Console.WriteLine($"\t{numberCriticals} Criticals");

            Console.ForegroundColor = startingForeground;


            if (errors is not null && errors.Count > 0)
            {
                Console.WriteLine(">");
                Console.WriteLine("Line\t| Message");
                errors.Sort();
                foreach (var error in errors)
                {
                    Console.ForegroundColor = ColorOfCriticality(error.Criticality);
                    Console.WriteLine($"{error.Line}\t| {error.Message}");
                }
                Console.WriteLine();
                Console.ForegroundColor = startingForeground;
            }
            Console.WriteLine();
        }

        static ConsoleColor ColorOfCriticality(ErrorCriticality crit) => crit switch
        {
            ErrorCriticality.Info => ConsoleColor.Cyan,
            ErrorCriticality.Warning => ConsoleColor.Yellow,
            ErrorCriticality.Error => ConsoleColor.Red,
            ErrorCriticality.Critical => ConsoleColor.DarkRed,
            _ => ConsoleColor.White
        };

        static void PrintCode(byte[] code)
        {
            string output = BitConverter.ToString(code).Replace("-", " ");
            Console.WriteLine("Generated output:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(output);
        }
    }
}
