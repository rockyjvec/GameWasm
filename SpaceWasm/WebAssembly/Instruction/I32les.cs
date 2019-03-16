using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32les : Instruction
    {
        public I32les(Parser parser) : base(parser, true)
        {
        }
    }
}
