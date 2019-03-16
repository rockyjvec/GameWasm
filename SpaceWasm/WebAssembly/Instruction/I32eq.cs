using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    internal class I32eq : Instruction
    {
        public I32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
