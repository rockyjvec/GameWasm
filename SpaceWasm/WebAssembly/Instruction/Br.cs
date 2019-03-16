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
        public Br(Parser parser) : base(parser, true)
        {
            this.labelidx = (int)parser.GetIndex();
        }
    }
}
