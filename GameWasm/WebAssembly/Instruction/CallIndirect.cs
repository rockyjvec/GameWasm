using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class CallIndirect : Instruction
    {
        int typeidx;
        public int tableidx;

        public CallIndirect(Parser parser) : base(parser, true)
        {
            typeidx = (int)parser.GetIndex();

            tableidx = (int)parser.GetUInt32();

            if(tableidx != 0x00)
            {
                Console.WriteLine("WARNING: call_indirect called with non-zero: 0x" + tableidx.ToString("X"));
            }
        }
           
        public override string ToString()
        {
            return "call_indirect";
        }
    }
}
