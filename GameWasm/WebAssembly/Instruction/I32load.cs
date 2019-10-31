using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load : Instruction
    {
        public UInt32 align, offset;

        public I32load(Parser parser) : base(parser, true)
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
