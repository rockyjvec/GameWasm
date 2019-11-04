using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64load16s : Instruction
    {
        public UInt32 align;

        public I64load16s(Parser parser) : base(parser, true)
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
