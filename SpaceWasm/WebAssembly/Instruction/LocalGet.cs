using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalGet : Instruction
    {
        int index;

        public LocalGet(Parser parser) : base(parser, true)
        {
            this.index = (int)parser.GetUInt32();
        }
    }
}
