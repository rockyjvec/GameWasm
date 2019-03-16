using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32eqz : Instruction
    {
        public I32eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
