using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32geu : Instruction
    {
        public I32geu(Parser parser) : base(parser, true)
        {
        }
    }
}
