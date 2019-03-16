using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32eq : Instruction
    {
        public F32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
