using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Model
{
    internal class MemoryReservation
    {
        public MemoryReservation(uint amountBytes, byte? value = null)
        {
            AmountBytes = amountBytes;
            Value = value;
        }

        public uint AmountBytes { get; set; }
        public byte? Value { get; set; }
    }
}
