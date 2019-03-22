using System;

namespace WebAssembly.Instruction
{
    internal class I32truncF32u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)Math.Truncate((float)store.Stack.PopF32()));

            return this.Next;
        }

        public I32truncF32u(Parser parser) : base(parser, true)
        {
        }
    }
}