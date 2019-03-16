using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64les : Instruction
    {
        public I64les(Parser parser) : base(parser, true)
        {
        }
    }
}
