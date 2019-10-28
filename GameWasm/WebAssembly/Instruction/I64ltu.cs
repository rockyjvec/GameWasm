﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64ltu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (UInt64)store.Stack.PopI64();
            var a = (UInt64)store.Stack.PopI64();

            if (a < b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I64ltu(Parser parser) : base(parser, true)
        {
        }
    }
}