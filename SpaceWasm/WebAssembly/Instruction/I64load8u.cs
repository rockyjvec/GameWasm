﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64load8u : Instruction
    {
        int align, offset;

        public I64load8u(Parser parser) : base(parser, true)
        {
            this.align = (int)parser.GetUInt32();
            this.offset = (int)parser.GetUInt32();
        }
    }
}
