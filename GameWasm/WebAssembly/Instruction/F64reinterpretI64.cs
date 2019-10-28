﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64reinterpretI64 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(BitConverter.ToDouble(BitConverter.GetBytes(store.Stack.PopI64()), 0));
            return Next;
        }

        public F64reinterpretI64(Parser parser) : base(parser, true)
        {
        }
    }
}