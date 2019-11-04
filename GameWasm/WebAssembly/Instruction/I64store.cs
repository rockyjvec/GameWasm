using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64store : Instruction
    {
        public UInt32 align;

        public I64store(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "i64.store " + offset;
        }
    }
}
