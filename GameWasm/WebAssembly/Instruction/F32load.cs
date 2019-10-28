using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32load : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Stack.Frame f)
        {
            f.Push(f.Function.Module.Memory[0].GetF32((UInt64)offset + (UInt64)f.PopI32()));
            return Next;
        }

        public F32load(Parser parser) : base(parser, true)
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
