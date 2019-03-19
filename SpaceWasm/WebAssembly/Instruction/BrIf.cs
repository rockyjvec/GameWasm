using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class BrIf : Instruction
    {
        int labelidx;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.Pop();
            if ((UInt32)v > 0 )
            {
                Instruction i = store.CurrentFrame.Labels.Pop();
                for(int j = 0; j < labelidx - 1; j++)
                {
                    i = store.CurrentFrame.Labels.Pop();
                }
                return i;
            }
            else
            {
                Instruction i = store.CurrentFrame.Labels.Pop();
                return this.Next.Next;
            }
        }

        public BrIf(Parser parser) : base(parser, true)
        {
            this.labelidx = (int)parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + this.labelidx + ")";
        }
    }
}
