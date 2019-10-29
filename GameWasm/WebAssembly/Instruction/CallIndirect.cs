using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class CallIndirect : Instruction
    {
        int typeidx;
        int tableidx;

        protected override Instruction Run(Stack.Frame f)
        {
            var index = f.PopI32();
            if(tableidx >= f.Function.Module.Tables.Count())
            {
                throw new Exception("Table index out of bounds.");
            }
            var funcidx = f.Function.Module.Tables[tableidx].Get(index);
            if(funcidx >= f.Function.Module.Functions.Count())
            {
                throw new Exception("Function index out of bounds.");
            }
            f.Function.Module.Store.CallFunction(f.Function.Module.Functions[(int)funcidx]);
            return Next;
        }

        public CallIndirect(Parser parser, Function f) : base(parser, f, true)
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
