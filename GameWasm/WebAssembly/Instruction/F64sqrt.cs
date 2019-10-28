﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64sqrt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();

            store.Stack.Push((double)Math.Sqrt((double)b));

            return Next;
        }

        public F64sqrt(Parser parser) : base(parser, true)
        {
        }
    }
}