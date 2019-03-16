using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class MemoryGrow : Instruction
    {
        public MemoryGrow(Parser parser) : base(parser, true)
        {
            UInt32 zero = parser.GetUInt32(); // May be used in future version of WebAssembly to address additional memories

            if(zero != 0x00)
            {
                Console.WriteLine("WARNING: memory.grow called with non-zero: 0x" + zero.ToString("X"));
            }
        }
    }
}
