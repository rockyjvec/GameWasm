using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32gts : Instruction
    {
        public I32gts(Parser parser) : base(parser, true)
        {
        }
    }
}
