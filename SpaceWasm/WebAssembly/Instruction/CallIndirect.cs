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
            return this.Next;
        }

        public CallIndirect(Parser parser) : base(parser, true)
        {
            this.typeidx = (int)parser.GetIndex();

            this.tableidx = (int)parser.GetUInt32();

            if(this.tableidx != 0x00)
            {
                Console.WriteLine("WARNING: call_indirect called with non-zero: 0x" + this.tableidx.ToString("X"));
            }
        }
    }
}
