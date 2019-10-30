using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64load32u : Instruction
    {
        public UInt32 align, offset;

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushI64(f.Function.Module.Memory[0].GetI6432u((UInt64)offset + (UInt64)f.PopI32()));
            return Next;
        }

        public I64load32u(Parser parser, Function f) : base(parser, f, true)
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
