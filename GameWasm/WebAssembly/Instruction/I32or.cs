﻿namespace GameWasm.Webassembly.Instruction
{
    internal class I32or : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopI32() | store.Stack.PopI32());
            return Next;
        }

        public I32or(Parser parser) : base(parser, true)
        {
        }
    }
}