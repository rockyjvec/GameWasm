using System;

namespace WebAssembly.Instruction
{
    internal class I64truncF64u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt64)Math.Truncate((double)store.Stack.PopF64()));

            return this.Next;
        }

        public I64truncF64u(Parser parser) : base(parser, true)
        {
        }
    }
}