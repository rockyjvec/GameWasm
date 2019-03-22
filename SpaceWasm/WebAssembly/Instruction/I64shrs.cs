﻿using System;

namespace WebAssembly.Instruction
{
    internal class I64shrs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI64();
            var a = (Int64)store.Stack.PopI64();

            store.Stack.Push(a >> b);
            return this.Next;
        }

        public I64shrs(Parser parser) : base(parser, true)
        {
        }
    }
}