﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32ne : Instruction
    {
        public F32ne(Parser parser) : base(parser, true)
        {
        }
    }
}
