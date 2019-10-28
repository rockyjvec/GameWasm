﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64nearest : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF64();

            store.Stack.Push((double)Math.Round(a));
            return Next;
        }

        public F64nearest(Parser parser) : base(parser, true)
        {
        }
    }
}