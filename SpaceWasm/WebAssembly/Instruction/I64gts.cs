using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64gts : Instruction
    {
        public I64gts(Parser parser) : base(parser, true)
        {
        }
    }
}
