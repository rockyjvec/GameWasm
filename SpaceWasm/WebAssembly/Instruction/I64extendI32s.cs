﻿using System;

namespace WebAssembly.Instruction
{
    internal class I64extendI32s : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            store.Stack.Push((UInt64)(Int64)((Int32)b));

            return this.Next;
        }

        public I64extendI32s(Parser parser) : base(parser, true)
        {
        }
    }
}