using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64load : Instruction
    {
        UInt32 align, offset;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.Function.Module.Memory[0].GetI64((UInt64)offset + (UInt64)f.PopI32()));
            return Next;
        }

        public I64load(Parser parser, Function f) : base(parser, f, true)
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
