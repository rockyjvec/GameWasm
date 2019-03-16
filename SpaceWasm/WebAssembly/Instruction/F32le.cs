using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32le : Instruction
    {
        public F32le(Parser parser) : base(parser, true)
        {
        }
    }
}
