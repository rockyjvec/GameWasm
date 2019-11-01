using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrIf : Instruction
    {
        public UInt32 labelidx;

        public BrIf(Parser parser) : base(parser, true)
        {
            labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return "br_if";
        }
    }
}
