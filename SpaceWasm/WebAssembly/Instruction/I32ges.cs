using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32ges : Instruction
    {
        public I32ges(Parser parser) : base(parser, true)
        {
        }
    }
}
