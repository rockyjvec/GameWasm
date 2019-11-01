using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load16s : Instruction
    {
        public UInt32 align, offset;

        public I32load16s(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "i32.load16_s " + offset;
        }
    }
}
