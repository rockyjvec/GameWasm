using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load8s : Instruction
    {
        public UInt32 align, offset;

        public I32load8s(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "i32.load8_s" + offset;
        }
    }
}
