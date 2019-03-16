using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32ne : Instruction
    {
        public I32ne(Parser parser) : base(parser, true)
        {
        }
    }
}
