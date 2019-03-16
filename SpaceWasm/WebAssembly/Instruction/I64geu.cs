using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64geu : Instruction
    {
        public I64geu(Parser parser) : base(parser, true)
        {
        }
    }
}
