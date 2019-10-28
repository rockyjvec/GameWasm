﻿namespace GameWasm.Webassembly.Instruction
{
    internal class I32add : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

            store.Stack.Push(a + b);

            return Next;
        }
        public I32add(Parser parser) : base(parser, true)
        {
            
        }
    }
}