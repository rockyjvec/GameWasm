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
