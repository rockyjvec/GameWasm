using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64ne : Instruction
    {
        public I64ne(Parser parser) : base(parser, true)
        {
        }
    }
}
