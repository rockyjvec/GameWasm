using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32store16 : Instruction
    {
        int align, offset;

        public I32store16(Parser parser) : base(parser, true)
        {
            this.align = (int)parser.GetUInt32();
            this.offset = (int)parser.GetUInt32();
        }
    }
}
