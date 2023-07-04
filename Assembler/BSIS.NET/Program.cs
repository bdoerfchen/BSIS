using BSIS.NET.Assembler;
using BSIS.NET.ISA;
using BSIS.NET.Model;

namespace BSIS.NET
{
    internal class Program
    {
        const string TEST_PATH = @"C:\Users\ben13\Desktop\test.txt";

        const string FLAG_WAITONEND = "-w";
        const string FLAG_NOOUTPUT = "-h";

        const uint PROGRAM_LENGTH_BUDGET = 10;

#if DEBUG
        const bool DEBUG_MODE = true;
#else
        const bool DEBUG_MODE = false;
#endif

        static void Main(string[] args)
        {
            //Assemble with length budget
            AssembleResult result;
            using (FileStream file = new FileStream(TEST_PATH, FileMode.Open, FileAccess.Read))
            {
                BSISAssembler assembler = new BSISAssembler(file);
                result = assembler.Assemble(PROGRAM_LENGTH_BUDGET);
            }

            //Output
            if (!HasFlag(FLAG_NOOUTPUT, args))
                AssemblyConsoleOutput.Output(result);

            //Wait after end
            if (DEBUG_MODE || HasFlag(FLAG_WAITONEND, args))
                Console.ReadKey();
        }

        static bool HasFlag(string flag, string[] args)
        {
            return args.Where(a => string.Equals(a, flag, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }
    }
}