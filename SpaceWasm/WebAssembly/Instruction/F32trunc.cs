using System;

namespace WebAssembly.Instruction
{
    internal class F32trunc : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Truncate((float)store.Stack.PopF32()));

            return this.Next;
        }


        public F32trunc(Parser parser) : base(parser, true)
        {
        }
    }
}