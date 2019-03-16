using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Select : Instruction
    {
        public Select(Parser parser) : base(parser, true)
        {
        }
    }
}
