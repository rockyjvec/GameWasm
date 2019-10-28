using System;
namespace GameWasm.Webassembly.Instruction
{
    class I32store8 : Instruction
    {
        UInt32 align, offset;

        protected override Instruction Run(Stack.Frame f)
        {
            var v = f.PopI32();
            var index = f.PopI32();
            f.Function.Module.Memory[0].Set((UInt64)offset + (UInt64)index, (byte)v);
            return Next;
        }

        public I32store8(Parser parser) : base(parser, true)
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
