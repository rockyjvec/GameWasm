using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32lt : Instruction
    {
        public F32lt(Parser parser) : base(parser, true)
        {
        }
    }
}
