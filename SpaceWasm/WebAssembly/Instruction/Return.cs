using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Return : Instruction
    {
        public override Instruction Run(Store store)
        {
            return null;
        }

        public Return(Parser parser) : base(parser, true)
        {
        }
    }
}
