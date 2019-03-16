using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32gtu : Instruction
    {
        public I32gtu(Parser parser) : base(parser, true)
        {
        }
    }
}
