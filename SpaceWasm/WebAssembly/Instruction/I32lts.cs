using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32lts : Instruction
    {
        public I32lts(Parser parser) : base(parser, true)
        {
        }
    }
}
