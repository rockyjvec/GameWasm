using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64ges : Instruction
    {
        public I64ges(Parser parser) : base(parser, true)
        {
        }
    }
}
