using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Unreachable : Instruction
    {
        public Unreachable(Parser parser) : base(parser, true)
        {
        }
    }
}
