using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32store : Instruction
    {
        public UInt32 align;

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
