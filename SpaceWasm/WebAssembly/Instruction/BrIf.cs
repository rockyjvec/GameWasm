using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class BrIf : Instruction
    {
        UInt32 labelidx;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.Pop();
            if ((UInt32)v > 0)
            {
                Stack.Label l = store.Stack.PopLabel(labelidx + 1);

                if(l.Instruction as Loop != null) return l.Instruction.Next;
                return l.Instruction.Next;
            }
            else
            {
                return this.Next;
            }
        }

        public BrIf(Parser parser) : base(parser, true)
        {
            this.labelidx = parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + this.labelidx + ")";
        }
    }
}
