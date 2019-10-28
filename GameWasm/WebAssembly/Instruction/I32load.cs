using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load : Instruction
    {
        UInt64 align, offset;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.Function.Module.Memory[0].GetI32(offset + (UInt64)f.PopI32()));
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
