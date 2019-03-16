using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64eqz : Instruction
    {
        public I64eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
