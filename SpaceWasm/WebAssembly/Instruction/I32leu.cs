using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32leu : Instruction
    {
        public I32leu(Parser parser) : base(parser, true)
        {
        }
    }
}
