using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrIf : Instruction
    {
        UInt32 labelidx;

        protected override Instruction Run(Stack.Frame f)
        {
            var v = f.Pop();
            if ((UInt32)v > 0)
            {
                Stack.Label l = f.PopLabel(labelidx + 1);

                if(l.Instruction as Loop != null) return l.Instruction;
                return l.Instruction.Next;
            }
            else
            {
                return Next;
            }
        }

        public BrIf(Parser parser) : base(parser, true)
        {
            labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + labelidx + ")";
        }
    }
}
