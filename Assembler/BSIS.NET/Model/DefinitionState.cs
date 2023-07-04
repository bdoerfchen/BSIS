using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Model
{
    internal class DefinitionState
    {
        public Dictionary<string, string> Constants { get; set; } = new();
        public Dictionary<string, MemoryReservation> Reservations { get; set; } = new();
        public List<CommandLine> Instructions { get; set; } = new();
        public uint ExpectedProgramLength { get; set; }

    }
}
