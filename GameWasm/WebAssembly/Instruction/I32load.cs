using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load : Instruction
    {
        public UInt32 align, offset;

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushI32(f.Function.Module.Memory[0].GetI32(offset + (UInt64)f.PopI32()));
            return Next;
        }

        public I32load(Parser parser, Function f) : base(parser, f, true)
        {
            align = parser.GetUInt32();
            offset = parser.GetUInt32();
        }

        public override string ToString()
        {
            return base.ToString() + "(offset = " + offset + ")";
        }
    }
}
