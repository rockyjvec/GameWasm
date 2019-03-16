using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Nop : Instruction
    {
        public override Instruction Run(Store store)
        {
            return this.Next;
        }

        public Nop(Parser parser) : base(parser, true)
        {
        }
    }
}
