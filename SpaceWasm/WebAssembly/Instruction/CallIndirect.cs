using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class CallIndirect : Instruction
    {
        int typeidx;
        public CallIndirect(Parser parser) : base(parser, true)
        {
            this.typeidx = (int)parser.GetIndex();

            UInt32 zero = parser.GetUInt32();

            if(zero != 0x00)
            {
                Console.WriteLine("WARNING: call_indirect called with non-zero: 0x" + zero.ToString("X"));
            }
        }
    }
}
