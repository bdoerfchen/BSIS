using BSIS.NET.ISA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSIS.NET.Model
{
    internal class AssembleResult
    {
        public byte[]? ByteCode { get; set; }
        public DateTime FinishedAt { get; set; } = DateTime.UtcNow;
        public List<AssemblingError> Errors { get; set; }

        public AssembleResult(byte[]? byteCode, List<AssemblingError> errors)
        {
            ByteCode = byteCode;
            Errors = errors;
        }

        public bool HasFailed() => ByteCode is null;
    }
}
