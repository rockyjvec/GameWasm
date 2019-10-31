using System;

namespace GameWasm.Webassembly.Instruction
{
    class Br : Instruction
    {
        public UInt32 labelidx;

        public Br(Parser parser) : base(parser, true)
        {
            labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + labelidx + ")";
        }
    }
}
