﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Block : Instruction
    {
        Instruction label = null;
        byte type;

        public override void End(Instruction end)
        {
            this.label = end;
        }

        public override Instruction Run(Store store)
        {
            store.Stack.Push(new Stack.Label(this.label, new byte[] { type }));
            return this.Next;
        }

        public Block(Parser parser) : base(parser, true)
        {
            this.type = parser.GetBlockType();
        }

        public override string ToString()
        {
            return "block";
        }
    }
}
