using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32store : Instruction
    {
        public UInt32 align;

        public I32store(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "i32.store " + offset;
        }
    }
}
