using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32gt : Instruction
    {
        public F32gt(Parser parser) : base(parser, true)
        {
        }
    }
}
