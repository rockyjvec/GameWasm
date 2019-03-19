using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Br : Instruction
    {
        int labelidx;
        public override Instruction Run(Store store)
        {
            Instruction i = store.CurrentFrame.Labels.Pop();
            for (int j = 0; j < labelidx - 1; j++)
            {
                i = store.CurrentFrame.Labels.Pop();
            }
            return i;
        }

        public Br(Parser parser) : base(parser, true)
        {
            this.labelidx = (int)parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + this.labelidx + ")";
        }
    }
}
