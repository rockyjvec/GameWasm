using System;

namespace WebAssembly.Instruction
{
    internal class I32truncF64u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)Math.Truncate((double)store.Stack.PopF64()));

            return this.Next;
        }

        public I32truncF64u(Parser parser) : base(parser, true)
        {
        }
    }
}