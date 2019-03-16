using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64lts : Instruction
    {
        public I64lts(Parser parser) : base(parser, true)
        {
        }
    }
}
