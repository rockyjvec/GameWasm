using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32store16 : Instruction
    {
        public UInt32 align, offset;

        public I32store16(Parser parser) : base(parser, true)
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
