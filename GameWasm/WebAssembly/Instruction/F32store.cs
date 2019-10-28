using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32store : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Stack.Frame f)
        {
            var v = f.PopF32();
            var index = f.PopI32();
            byte[] bytes = BitConverter.GetBytes(v);
            f.Function.Module.Memory[0].SetBytes((UInt64)offset + (UInt64)index, bytes);
            return Next;
        }

        public F32store(Parser parser) : base(parser, true)
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
