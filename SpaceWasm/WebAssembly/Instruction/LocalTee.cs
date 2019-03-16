using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalTee : Instruction
    {
        int localidx;

        public LocalTee(Parser parser) : base(parser, true)
        {
            this.localidx = (int)parser.GetUInt32();
        }
    }
}
