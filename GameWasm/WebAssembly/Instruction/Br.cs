using System;

namespace GameWasm.Webassembly.Instruction
{
    class Br : Instruction
    {
        UInt32 labelidx;
        protected override Instruction Run(Stack.Frame f)
        {
            Stack.Label l = f.PopLabel(labelidx + 1);

            if (l.Instruction as Loop != null) return l.Instruction;

            return l.Instruction.Next;
        }

        public Br(Parser parser, Function f) : base(parser, f, true)
        {
            labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + labelidx + ")";
        }
    }
}
