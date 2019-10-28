using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64store : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Stack.Frame f)
        {
            var v = (UInt64)f.PopI64();
            var index = f.PopI32();
            f.Function.Module.Memory[0].SetI64((UInt64)(offset + index), v);

            return Next;
        }

        public I64store(Parser parser) : base(parser, true)
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
