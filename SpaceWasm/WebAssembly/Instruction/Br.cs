using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Br : Instruction
    {
        UInt32 labelidx;
        public override Instruction Run(Store store)
        {
            Stack.Label l = store.Stack.PopLabel(labelidx + 1);
            if (l.Instruction as Loop != null) return l.Instruction;
            return l.Instruction.Next;
        }

        public Br(Parser parser) : base(parser, true)
        {
            this.labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + this.labelidx + ")";
        }
    }
}
