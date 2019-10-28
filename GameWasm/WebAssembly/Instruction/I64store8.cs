using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64store8 : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopI64();
            var index = store.Stack.PopI32();
            store.CurrentFrame.Module.Memory[0].Set((UInt64)offset + (UInt64)index, (byte)v);
            return Next;
        }

        public I64store8(Parser parser) : base(parser, true)
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
