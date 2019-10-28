﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64min : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            store.Stack.Push((double)Math.Min(a, b));
            return Next;
        }

        public F64min(Parser parser) : base(parser, true)
        {
        }
    }
}