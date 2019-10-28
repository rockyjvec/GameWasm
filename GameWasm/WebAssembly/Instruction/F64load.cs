using System;

namespace GameWasm.Webassembly.Instruction
{
    class F64load : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.CurrentFrame.Module.Memory[0].GetF64((UInt64)offset + (UInt64)store.Stack.PopI32()));
            return Next;
        }

        public F64load(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return base.ToString() + "(offset = " + offset + ")";
        }
    }
}
