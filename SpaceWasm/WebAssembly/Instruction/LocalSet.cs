using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalSet : Instruction
    {
        int localidx;

        public LocalSet(Parser parser) : base(parser, true)
        {
            this.localidx = (int)parser.GetUInt32();
        }
    }
}
