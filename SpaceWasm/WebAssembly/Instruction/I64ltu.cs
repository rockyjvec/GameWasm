using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64ltu : Instruction
    {
        public I64ltu(Parser parser) : base(parser, true)
        {
        }
    }
}
