using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load : Instruction
    {
        UInt64 align, offset;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.CurrentFrame.Module.Memory[0].GetI32(offset + (UInt64)store.Stack.PopI32()));
            return Next;
        }

        public I32load(Parser parser) : base(parser, true)
        {
            align = (UInt64)parser.GetUInt32();
            offset = (UInt64)parser.GetUInt32();
        }

        public override string ToString()
        {
            return base.ToString() + "(offset = " + offset + ")";
        }
    }
}
