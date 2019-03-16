using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F64store : Instruction
    {
        int align, offset;

        public F64store(Parser parser) : base(parser, true)
        {
            this.align = (int)parser.GetUInt32();
            this.offset = (int)parser.GetUInt32();
        }
    }
}
