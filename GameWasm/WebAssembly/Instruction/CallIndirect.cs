using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class CallIndirect : Instruction
    {
        int typeidx;
        int tableidx;

        public override Instruction Run(Store store)
        {
            var index = store.Stack.PopI32();
            if(tableidx >= store.CurrentFrame.Module.Tables.Count())
            {
                throw new Exception("Table index out of bounds.");
            }
            var funcidx = store.CurrentFrame.Module.Tables[tableidx].Get(index);
            if(funcidx >= store.CurrentFrame.Module.Functions.Count())
            {
                throw new Exception("Function index out of bounds.");
            }
            store.CurrentFrame.Module.Functions[(int)funcidx].NativeCall();
            return Next;
        }

        public CallIndirect(Parser parser) : base(parser, true)
        {
            typeidx = (int)parser.GetIndex();

            tableidx = (int)parser.GetUInt32();

            if(tableidx != 0x00)
            {
                Console.WriteLine("WARNING: call_indirect called with non-zero: 0x" + tableidx.ToString("X"));
            }
        }
    }
}
