using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64gtu : Instruction
    {
        public I64gtu(Parser parser) : base(parser, true)
        {
        }
    }
}
